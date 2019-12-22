//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Domain.Github;
using BookGen.Utilities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_UpdateUtils
    {
        private List<Release> _releasesStub;
        private Mock<ILog> _log;

        [SetUp]
        public void Setup()
        {
            _log = new Mock<ILog>();
            _releasesStub = new List<Release>
            {
                new Release
                {
                    IsDraft = false,
                    PublishDate =  DateTime.Now.AddDays(1),
                    Body = "Newer update",
                    IsPreRelase = false,
                    Name = "notprerelase",
                    Assets = new List<Asset>
                    {
                        new Asset
                        {
                            Size = 1234,
                            Name = "file.zip",
                            ContentType = "application/x-zip-compressed",
                            DownloadUrl = "notdownloadable"
                        }
                    }
                },
                new Release
                {
                    IsDraft = false,
                    PublishDate =  DateTime.Now.AddDays(10),
                    Body = "newer prerelease",
                    IsPreRelase = true,
                    Name = "prerelase",
                    Assets = new List<Asset>
                    {
                        new Asset
                        {
                            Size = 1234,
                            Name = "file.zip",
                            ContentType = "application/x-zip-compressed",
                            DownloadUrl = "notdownloadable"
                        },
                        new Asset
                        {
                            Size = 4892,
                            Name = "file.bin",
                            ContentType = "asd",
                            DownloadUrl = "url"
                        }
                    }
                },
            };
        }

        [TearDown]
        public void Teardown()
        {
            _releasesStub = null;
            _log = null;
        }


        [Test]
        public void EnsureThat_UpdateUtils_GetAssemblyLinkerDate_ReturnsCorrectDate()
        {
            bool result = UpdateUtils.GetAssemblyLinkerDate(out DateTime date);

            Assert.IsTrue(result);
            Assert.AreEqual(date, DateTime.Now.Date);

        }

        [Test]
        public void EnsureThat_UpdateUtils_SelectLatestRelease_ReturnsCorrect_NoPrerelease()
        {
           Release? release =  UpdateUtils.SelectLatestRelease(_releasesStub, false);

            Assert.NotNull(release);
            Assert.AreEqual("notprerelase", release.Name);
        }

        [Test]
        public void EnsureThat_UpdateUtils_SelectLatestRelease_ReturnsCorrect_Prerelease()
        {
            Release release = UpdateUtils.SelectLatestRelease(_releasesStub, true);

            Assert.NotNull(release);
            Assert.AreEqual("prerelase", release.Name);
        }

        [Test]
        public void EnsureThat_UpdateUtils_SelectAssetToDownload_ReturnsZipAsset()
        {
            Release release = UpdateUtils.SelectLatestRelease(_releasesStub, true);
            Asset asset = UpdateUtils.SelectAssetToDownload(release);

            Assert.NotNull(asset);
            Assert.AreEqual("file.zip", asset.Name);
        }

        [Test]
        public void EnsureThat_UpdateUtils_GetGithubReleases_DownloadsResources()
        {
            const string url = "https://api.github.com/repos/webmaster442/BookGen/releases";
            bool result = UpdateUtils.GetGithubReleases(url, _log.Object, out List<Release> releases);

            Assert.IsTrue(result);
            Assert.IsNotNull(releases);
            Assert.IsTrue(releases.Count > 0);
        }
    }
}
