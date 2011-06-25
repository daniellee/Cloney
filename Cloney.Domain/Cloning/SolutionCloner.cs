using System;
using System.Collections.Generic;
using System.IO;
using Cloney.Domain.Cloning.Abstractions;
using Cloney.Domain.Extensions;
using Cloney.Domain.IO;
using Cloney.Domain.IO.Abstractions;

namespace Cloney.Domain.Cloning
{
    /// <summary>
    /// This class can be used to clone a Visual Studio solution.
    /// </summary>
    /// <remarks>
    /// The class can be instructed to exclude certain files and
    /// folders. It can also be told to direct copy certain file
    /// types, instead of the default "deep copy", which creates
    /// new files and fills them with modified content.
    /// </remarks>
    public class SolutionCloner : ICanCloneSolution
    {
        public SolutionCloner(IEnumerable<string> excludeFolderPatterns, IEnumerable<string> excludeFilePatterns, IEnumerable<string> plainCopyFilePatterns)
            : this(excludeFolderPatterns, excludeFilePatterns, plainCopyFilePatterns, new PathPatternMatcher(), new NamespaceReplacer())
        {
        }

        public SolutionCloner(IEnumerable<string> excludeFolderPatterns, IEnumerable<string> excludeFilePatterns, IEnumerable<string> plainCopyFilePatterns, ICanMatchPathPattern pathPatternMatcher, ICanReplaceNamespace namespaceReplacer)
        {
            ExcludeFolderPatterns = excludeFolderPatterns;
            ExcludeFilePatterns = excludeFilePatterns;
            PlainCopyFilePatterns = plainCopyFilePatterns;

            PathPatternMatcher = pathPatternMatcher;
            NamespaceReplacer = namespaceReplacer;
        }


        public string CurrentPath { get; private set; }

        public IEnumerable<string> ExcludeFolderPatterns { get; private set; }

        public IEnumerable<string> ExcludeFilePatterns { get; private set; }

        public ICanReplaceNamespace NamespaceReplacer { get; private set; }

        public ICanMatchPathPattern PathPatternMatcher { get; private set; }

        public IEnumerable<string> PlainCopyFilePatterns { get; private set; }


        public event EventHandler CloningBegun;

        public event EventHandler CloningEnded;


        public string AdjustPath(string path, string sourcePath, string sourceNamespace, string targetNamespace)
        {
            path = path.Replace(sourcePath, "");
            return NamespaceReplacer.ReplaceNamespace(path, sourceNamespace, targetNamespace);
        }

        public bool IsExcludedFolder(string folderPath)
        {
            return PathPatternMatcher.IsAnyMatch(folderPath, ExcludeFolderPatterns);
        }

        public bool IsExcludedFile(string filePath)
        {
            return PathPatternMatcher.IsAnyMatch(filePath, ExcludeFilePatterns);
        }

        public bool IsPlainCopyFile(string filePath)
        {
            return PathPatternMatcher.IsAnyMatch(filePath, PlainCopyFilePatterns);
        }


        public void CloneSolution(string sourcePath, string sourceNamespace, string targetPath, string targetNamespace)
        {
            OnCloningBegun(null);
            
            CopySubFolders(sourcePath, sourcePath, sourceNamespace, targetPath, targetNamespace);
            CopyFolderFiles(sourcePath, sourcePath, sourceNamespace, targetPath, targetNamespace);
            CurrentPath = "";

            OnCloningEnded(null);
        }
        
        private void CopyFolderFiles(string folderPath, string sourcePath, string sourceNamespace, string targetPath, string targetNamespace)
        {
            if (folderPath.IsNullOrEmpty())
                return;

            foreach (var filePath in Directory.GetFiles(folderPath))
            {
                CurrentPath = filePath;
                
                var fileName = new FileInfo(filePath).Name;
                if (IsExcludedFile(fileName))
                    continue;

                var adjustedFilePath = AdjustPath(filePath, sourcePath, sourceNamespace, targetNamespace);
                var targetFilePath = targetPath + adjustedFilePath;

                if (IsPlainCopyFile(fileName))
                {
                    File.Copy(filePath, targetFilePath, true);
                    continue;
                }

                var sourceStream = new StreamReader(filePath);
                var sourceContent = sourceStream.ReadToEnd();
                sourceStream.Close();

                if (!sourceContent.Contains(sourceNamespace))
                {
                    File.Copy(filePath, targetFilePath, true);
                    continue;
                }

                var targetStream = new StreamWriter(targetFilePath);
                targetStream.Write(NamespaceReplacer.ReplaceNamespace(sourceContent, sourceNamespace, targetNamespace));
                targetStream.Close();
            }
        }

        private void CopySubFolders(string parentFolderPath, string sourcePath, string sourceNamespace, string targetPath, string targetNamespace)
        {
            if (parentFolderPath.IsNullOrEmpty())
                return;

            foreach (var directory in Directory.GetDirectories(parentFolderPath))
            {
                CurrentPath = directory;

                var folderName = new DirectoryInfo(directory).Name;
                if (IsExcludedFolder(folderName))
                    continue;

                var adjustedFolderPath = AdjustPath(directory, sourcePath, sourceNamespace, targetNamespace);
                var targetFolderPath = targetPath + adjustedFolderPath;

                if (!Directory.Exists(targetFolderPath))
                    Directory.CreateDirectory(targetFolderPath);

                CopySubFolders(directory, sourcePath, sourceNamespace, targetPath, targetNamespace);
                CopyFolderFiles(directory, sourcePath, sourceNamespace, targetPath, targetNamespace);
            }
        }

        private void OnCloningBegun(EventArgs e)
        {
            if (CloningBegun != null)
                CloningBegun(this, e);
        }

        private void OnCloningEnded(EventArgs e)
        {
            if (CloningEnded != null)
                CloningEnded(this, e);
        }
    }
}
