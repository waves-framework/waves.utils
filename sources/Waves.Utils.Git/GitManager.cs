using System;
using System.Collections.Generic;
using System.Linq;
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
        
        /// <summary>
        /// Commit all changes.
        /// </summary>
        /// <param name="repoPath">Path to repository.</param>
        /// <param name="message">Commit message.</param>
        /// <param name="userName">User name.</param>
        /// <param name="email">Email.</param>
        public static void Commit(string repoPath, string message, string userName, string email)
        {
            var author = new Signature(userName, email, DateTime.Now);
            var committer = author;
            
            using var repo = new Repository(repoPath);
            StageChanges(repo);
            repo.Commit(message, author, committer);
        }

        /// <summary>
        /// Pushes to remote repository for current branch.
        /// </summary>
        /// <param name="repoPath">Path to repository.</param>
        /// <param name="remoteName">Remote name.</param>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        public static void Push(
            string repoPath, 
            string remoteName, 
            string userName, 
            string password)
        {
            using var repo = new Repository(repoPath);
            var remote = repo.Network.Remotes[remoteName];
            var options = new PushOptions
            {
                CredentialsProvider = (url, user, cred) =>
                    new UsernamePasswordCredentials {Username = userName, Password = password},
            };
            

            var branch = repo.Head.TrackedBranch;
            repo.Network.Push(remote, branch.UpstreamBranchCanonicalName, options);
        }

        /// <summary>
        /// Stages changes for current repository.
        /// </summary>
        /// <param name="repository"></param>
        public static void StageChanges(Repository repository)
        {
            var status = repository.RetrieveStatus();
            var filePaths = status.Modified.Select(mods => mods.FilePath).ToList();
            foreach (var path in filePaths) 
                Commands.Stage(repository, path);
        }
    }
}