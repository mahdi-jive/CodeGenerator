using CodeGenerator.FileSystem.Physical.Events;
using MediatR;

namespace CodeGenerator.FileSystem.Physical
{
    public class FileSystemEventHandler :
        INotificationHandler<FileRenamedNotification>,
        INotificationHandler<FileDeletedNotification>,
        INotificationHandler<FileContentUpdatedNotification>,
        INotificationHandler<FileAddedNotification>,
        INotificationHandler<DirectoryRenamedNotification>,
        INotificationHandler<DirectoryDeletedNotification>,
        INotificationHandler<DirectoryAddedNotification>
    {
        public Task Handle(FileRenamedNotification notification, CancellationToken cancellationToken)
        {
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