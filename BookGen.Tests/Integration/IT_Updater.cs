//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain.Github;
using BookGen.Framework.Server;
using BookGen.Tests.Environment;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BookGen.Tests.Integration
{
    [TestFixture, SingleThreaded]
    public class IT_Updater
    {
        private Updater _sut;
        private Mock<ILog> _logMock;
        private Release _release;
        private HttpTestServer _server;
        private string _temp;
        private string _newProgTemp;

        [OneTimeSetUp]
        public void Setup()
        {
            _temp = Path.Combine(Path.GetTempPath(), "updateTest");
            _newProgTemp = Path.Combine(_temp, "new");


            _logMock = new Mock<ILog>();
            _sut = new Updater(_logMock.Object, _temp, _newProgTemp);
            _release = new Release
            {
                PublishDate = DateTime.Now.AddDays(1),
                IsDraft = false,
                IsPreRelase = false,
                Name = "Update",
                Body = "some text",
                Assets = new List<Asset>
                {
                    new Asset
                    {
                        ContentType = "application/x-zip-compressed",
                        DownloadUrl = "http://localhost:8080/update.zip",
                        Name = "zip",
                        Size = 1234
                    },
                }
            };
            _server = new HttpTestServer(TestEnvironment.GetTestFolder(), 8080, _logMock.Object);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _sut = null;
            _server.Dispose();
            _server = null;
            _logMock = null;

            if (Directory.Exists(_temp))
            {
                Directory.Delete(_temp, true);
            }
        }

        [Test, Order(1)]
        public void Setup_CreateFoldersForTest()
        {
            bool exists = Directory.Exists(_temp);

            if (!exists)
                Directory.CreateDirectory(_temp);

            if (!Directory.Exists(_newProgTemp))
                Directory.CreateDirectory(_newProgTemp);

            Assert.IsTrue(Directory.Exists(_temp));
            Assert.IsTrue(Directory.Exists(_newProgTemp));
        }

        [Test, Order(2)]
        public void Setup_CreateDumyFiles()
        {
            string[] files = new string[] { "file1.txt", "file2.txt", "file3.txt", "file4.txt" };
            foreach (var file in files)
            {
                var path = Path.Combine(_temp, file);
                using StreamWriter f = File.CreateText(path);
                Assert.IsTrue(File.Exists(path));
            }
        }

        [Test, Order(3)]
        public void EnsureThat_Updater_UpdateProgram_UpdatesProgram()
        {
            var releases = new List<Release>
            {
                _release
            };

            Task<bool> t = _sut.UpdateProgram(false, "asd.exe", releases);
            t.Wait();

            Assert.IsTrue(t.Result);
        }

        [Test, Order(4)]
        public void Verify_FilesOverWritten()
        {
            Thread.Sleep(5 * 1024);
            var files = Directory.GetFiles(_temp, "*.txt");
            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                Assert.AreEqual(8 * 1024, fi.Length);
            }
        }

        [Test, Order(5)]
        public void Verify_NewFolderEmpty()
        {
            Assert.IsFalse(Directory.Exists(_newProgTemp));
        }
    }
}
