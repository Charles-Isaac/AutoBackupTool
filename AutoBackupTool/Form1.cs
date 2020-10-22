using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AutoBackupTool
{
    public partial class Form1 : Form
    {

        public static string SaveFolder;
        public static string SourceFolderPath;
        public static string TestFolder;
        public static List<string> failedFiles = new List<string>();
        private static string[] fileEntries;
        private static Map<string, string> GuidAndFilePath;
        private static Dictionary<string, uint> GuidWithCrcDict;

        private static long TotalToRead = 0;
        private static long TotalReadAndParsed = 0;
        private static string LocalGuidsAndCrcFilePath;
        private static string LocalFileGuidFilePath;

        public Form1()
        {
            InitializeComponent();


            bw1.ProgressChanged += Bw1_ProgressChanged; ;
            bw1.WorkerReportsProgress = true;
        }

        private void Bw1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pb1.Value = Math.Min(e.ProgressPercentage, 100);
            lblProgress.Text = $"{TotalReadAndParsed / (1024.0 * 1024.0):0.00}MB / {TotalToRead / (1024.0 * 1024.0):0.00}MB";
        }

        private void btnExploreFolder_Click(object sender, EventArgs e)
        {
            DialogResult dr = fbdFolder.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                txtFolderPath.Text = fbdFolder.SelectedPath;
            }
        }

        private void btnExploreSaveFolder_Click(object sender, EventArgs e)
        {
            DialogResult dr = fbdSaveFolder.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                txtSaveFolderPath.Text = fbdSaveFolder.SelectedPath;
            }
        }
        private void btnExploreTestFolder_Click(object sender, EventArgs e)
        {
            DialogResult dr = fbdTestFolder.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                txtTestFolderPath.Text = fbdTestFolder.SelectedPath;
            }
        }

        private void SetControlsEnabled(bool value)
        {
            txtFolderPath.Enabled = value;
            txtSaveFolderPath.Enabled = value;
            txtTestFolderPath.Enabled = value;
            btnExploreFolder.Enabled = value;
            btnExploreSaveFolder.Enabled = value;
            btnExploreTestFolder.Enabled = value;
            btnFindFiles.Enabled = value;
            btnTestSave.Enabled = value;
        }

        private void btnFindFiles_Click(object sender, EventArgs e)
        {
            SetControlsEnabled(false);
            failedFiles = new List<string>();

            SourceFolderPath = txtFolderPath.Text;
            SaveFolder = txtSaveFolderPath.Text;

            LocalGuidsAndCrcFilePath = $"{SaveFolder}\\LocalGuidsCrc.protopaths";
            LocalFileGuidFilePath = $"{SaveFolder}\\LocalFileGuid.protomap";

            DirectoryInfo directory = new DirectoryInfo(SourceFolderPath);
            DirectoryInfo[] directories = directory.GetDirectories("*", SearchOption.AllDirectories);
            //string[] directoriesPath = directories.Select(x => x.FullName.Substring(SourceFolderPath.Length)).ToArray();

            fileEntries = directory.GetFiles("*", SearchOption.AllDirectories).Select(x => x.FullName.Substring(SourceFolderPath.Length)).ToArray();

            //Dictionary<string, uint> GuidWithCrcDict = new Dictionary<string, uint>();

            Parallel.ForEach(fileEntries, (filePath) =>
            {
                Interlocked.Add(ref TotalToRead, new System.IO.FileInfo(SourceFolderPath + filePath).Length);
            });

            try
            {
                using (var file = File.OpenRead(LocalGuidsAndCrcFilePath))
                {
                    GuidWithCrcDict = ProtoBuf.Serializer.Deserialize<Dictionary<string, uint>>(file);
                }
            }
            catch (Exception)
            {
                GuidWithCrcDict = new Dictionary<string, uint>();
            }



            try
            {
                using (var file = File.OpenRead(LocalFileGuidFilePath))
                {
                    GuidAndFilePath = ProtoBuf.Serializer.Deserialize<Map<string, string>>(file);
                }
            }
            catch (Exception)
            {
                GuidAndFilePath = new Map<string, string>();
            }
            // interlock.increment

            bw1.DoWork += Bw1_DoWork;

            bw1.RunWorkerCompleted += Bw1_RunWorkerCompleted;
            bw1.RunWorkerAsync();

        }

        private void Bw1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bw1.DoWork -= Bw1_DoWork;
            bw1.RunWorkerCompleted -= Bw1_RunWorkerCompleted;

            using (var file = File.Create(LocalGuidsAndCrcFilePath))
            {
                ProtoBuf.Serializer.Serialize(file, GuidWithCrcDict);
            }
            using (var file = File.Create(LocalFileGuidFilePath))
            {
                ProtoBuf.Serializer.Serialize(file, GuidAndFilePath);
            }
            SetControlsEnabled(true);
        }

        private void Bw1_RunWorkerCompletedTest(object sender, RunWorkerCompletedEventArgs e)
        {
            bw1.DoWork -= Bw1_DoWorkTest;
            bw1.RunWorkerCompleted -= Bw1_RunWorkerCompletedTest;

            SetControlsEnabled(true);
        }

        private void Bw1_DoWork(object sender, DoWorkEventArgs e)
        {
            Parallel.ForEach(fileEntries, (filePath) =>
            {

                using (FileStream fs = File.OpenRead(SourceFolderPath + filePath))
                {
                    string fileGuid;
                    if (!GuidAndFilePath.Forward.TryGetValue(filePath, out fileGuid))
                    {
                        fileGuid = Guid.NewGuid().ToString();
#if DEBUG
                        if (GuidAndFilePath.Reverse.ContainsKey(fileGuid))
                        {
                            Debugger.Break();
                        }
#endif

                        GuidAndFilePath.Add(filePath, fileGuid);
                    }


                    uint PreEncryptionCrc = 0;
                    byte[] buff = new byte[4096];
                    int read = 0;
                    while ((read = fs.Read(buff, 0, buff.Length)) != 0)
                    {
                        PreEncryptionCrc = Crc32C.Crc32CAlgorithm.Append(PreEncryptionCrc, buff, 0, read);
                    }
                    fs.Position = 0;



                    if (GuidWithCrcDict.TryGetValue(fileGuid, out uint SavedCrc))
                    {
                        if (SavedCrc != PreEncryptionCrc)
                        {
                            try
                            {
                                uint Crc = EncryptionHelper.CompressAndEncryptAES(fs, SaveFolder, fileGuid, "potato");
                                if (Crc == PreEncryptionCrc)
                                {
                                    GuidWithCrcDict[fileGuid] = PreEncryptionCrc;
                                }
                            }
                            catch (Exception)
                            {
                                lock (failedFiles)
                                {
                                    failedFiles.Add(fileGuid);
                                }
                            }
                            //TODO: Send data
                            // if send successful

                        }
                    }
                    else
                    {
                        try
                        {
                            uint Crc = EncryptionHelper.CompressAndEncryptAES(fs, SaveFolder, fileGuid, "potato");
                            if (Crc == PreEncryptionCrc)
                            {
                                GuidWithCrcDict[fileGuid] = PreEncryptionCrc;
                            }
                        }
                        catch (Exception)
                        {
                            lock (failedFiles)
                            {
                                failedFiles.Add(fileGuid);
                            }
                        }
                    }
                    long a = Interlocked.Add(ref TotalReadAndParsed, fs.Length);
                    bw1.ReportProgress((int)(a * 100.0 / TotalToRead));

                }
            });
        }



        private void Bw1_DoWorkTest(object sender, DoWorkEventArgs e)
        {

            string protoMapFileLocation = $"{SaveFolder}\\LocalFileGuid.protomap";
            DirectoryInfo directory = new DirectoryInfo(SaveFolder);
            string[] fileEntries = directory.GetFiles("*.proto", SearchOption.AllDirectories).Select(x => x.FullName).ToArray();


            Map<string, string> GuidAndFilePath;
            using (var file = File.OpenRead(protoMapFileLocation))
            {
                GuidAndFilePath = ProtoBuf.Serializer.Deserialize<Map<string, string>>(file);
            }

            TotalToRead = 0; 
            TotalReadAndParsed = 0;
            Parallel.ForEach(fileEntries, (filePath) =>
            {
                Interlocked.Add(ref TotalToRead, new System.IO.FileInfo(filePath).Length);
            });


            Parallel.ForEach(fileEntries, (filePath) =>
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    long a = Interlocked.Add(ref TotalReadAndParsed, fs.Length);
                    bw1.ReportProgress((int)(a * 100.0 / TotalToRead));


                    int pTo = filePath.IndexOf(".proto");
                    int pFrom = filePath.LastIndexOf("\\") + 1;
                    string guid = filePath.Substring(pFrom, pTo - pFrom);
                    if (GuidAndFilePath.Reverse.TryGetValue(guid, out string Path))
                    {
                        try
                        {
                            EncryptionHelper.DecompressAndDecryptAES(fs, $"{TestFolder}\\{Path}", "potato");
                        }
                        catch (Exception)
                        {
                            lock (failedFiles)
                            {
                                failedFiles.Add($"{TestFolder}\\{Path}");
                            }
                        }
                    }
                    else
                    {
                        lock (failedFiles)
                        {
                            failedFiles.Add(guid);
                        }
                    }
                }
            });
        }


        private void btnTestSave_Click(object sender, EventArgs e)
        {
            failedFiles = new List<string>();

            SaveFolder = txtSaveFolderPath.Text;
            TestFolder = txtTestFolderPath.Text;


            bw1.DoWork += Bw1_DoWorkTest;
            bw1.RunWorkerCompleted -= Bw1_RunWorkerCompletedTest;

            bw1.RunWorkerAsync();
        }
    }
}
