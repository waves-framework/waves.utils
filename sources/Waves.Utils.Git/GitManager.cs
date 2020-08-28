using System;
using LibGit2Sharp;

namespace Waves.Utils.Git
{
    /// <summary>
    /// Git manager.
    /// </summary>
    public static class GitManager
    {
        /// <summary>
        /// Resets current commit.
        /// </summary>
        /// <param name="repoPath">Path to repository.</param>
        public static void Reset(string repoPath)
        {
            using var repo = new Repository(repoPath);
            var currentCommit = repo.Head.Tip;
            repo.Reset(ResetMode.Mixed, currentCommit);
        }
    }
}