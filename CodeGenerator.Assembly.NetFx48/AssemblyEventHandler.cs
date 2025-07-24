using CodeGenerator.Assembly.Events;
using MediatR;

namespace CodeGenerator.Assembly.NetFx48
{
    public class AssemblyEventHandler :
        INotificationHandler<AssemblyRenamedNotification>,
        INotificationHandler<AssemblyDeletedNotification>,
        INotificationHandler<AssemblyContentUpdatedNotification>,
        INotificationHandler<AssemblyAddedNotification>
    {
        public Task Handle(AssemblyRenamedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Assembly] {notification.Assembly.Directory.RelativePath} Renamed from {notification.OldName} to {notification.NewName}");
            return Task.CompletedTask;
        }

        public Task Handle(AssemblyDeletedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Assembly] Deleted: {notification.Assembly.Directory.RelativePath}");
            return Task.CompletedTask;
        }

        public Task Handle(AssemblyContentUpdatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Assembly] Content updated: {notification.Assembly.Directory.RelativePath}");
            return Task.CompletedTask;
        }

        public Task Handle(AssemblyAddedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Assembly] Added: {notification.Assembly.CsprojFile.RelativePath}");
            return Task.CompletedTask;
        }
    }
}