using CodeGenerator.Assembly.NetFx48.Extensions;
using CodeGenerator.FileSystem.Abstractions;
using CodeGenerator.FileSystem.Physical.Events;
using MediatR;
namespace CodeGenerator.Assembly.NetFx48
{

    public class FileSystemEventHandlerManager :
        INotificationHandler<FileRenamedNotification>,
        INotificationHandler<FileDeletedNotification>,
        INotificationHandler<FileContentUpdatedNotification>,
        INotificationHandler<FileAddedNotification>,
        INotificationHandler<DirectoryRenamedNotification>,
        INotificationHandler<DirectoryDeletedNotification>,
        INotificationHandler<DirectoryAddedNotification>
    {
        private readonly List<AssemblyNetFx48> _projects = new();

        private AssemblyNetFx48 GetProjectBySubPath(IFullPath fullPath)
        {
            return _projects.FirstOrDefault(p => p.Directory.IsPathInFolder(fullPath));
        }

        //private static bool IsPathInFolder(string folderPath, string filePath)
        //{
        //    // مسیر فولدر رو به مسیر کامل و استاندارد تبدیل می‌کنیم
        //    string folderFullPath = Path.GetFullPath(folderPath)
        //        .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
        //        + Path.DirectorySeparatorChar;

        //    // مسیر فایل رو به مسیر کامل و استاندارد تبدیل می‌کنیم
        //    string fileFullPath = Path.GetFullPath(filePath);

        //    // بررسی می‌کنیم که آیا مسیر فایل با مسیر فولدر شروع می‌شود یا نه
        //    return fileFullPath.StartsWith(folderFullPath, StringComparison.OrdinalIgnoreCase);
        //}
        public void RegisterProject(AssemblyNetFx48 project)
        {
            _projects.Add(project);
        }

        public Task Handle(FileRenamedNotification notification, CancellationToken cancellationToken)
        {

            var project = GetProjectBySubPath(notification.File);

            //if (project != null)
            //{
            //    await project.UpdateCsprojFileRenamedAsync(notification);
            //}
            Console.WriteLine($"[File] {notification.File.RelativePath} Renamed from {notification.OldName} to {notification.NewName}");
            return Task.CompletedTask;
        }

        public Task Handle(FileDeletedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[File] Deleted: {notification.File.RelativePath}");
            return Task.CompletedTask;
        }

        public Task Handle(FileContentUpdatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[File] Content updated: {notification.File.RelativePath}");
            return Task.CompletedTask;
        }

        public Task Handle(FileAddedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[File] Added: {notification.File.RelativePath}");
            return Task.CompletedTask;
        }

        public Task Handle(DirectoryRenamedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Directory] {notification.Directory.RelativePath} Renamed from {notification.OldName} to {notification.NewName}");
            return Task.CompletedTask;
        }

        public Task Handle(DirectoryDeletedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Directory] Deleted: {notification.Directory.RelativePath}");
            return Task.CompletedTask;
        }

        public Task Handle(DirectoryAddedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Directory] Added: {notification.Directory.RelativePath}");
            return Task.CompletedTask;
        }
    }
}