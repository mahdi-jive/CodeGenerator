using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Events;
using CodeGenerator.FileSystem.Abstractions;
using MediatR;
using System.Xml.Linq;

namespace CodeGenerator.Assembly.NetFx48
{
    public class AssemblyNetFx48 : IAssembly
    {
        public IMediator Mediator { get; private set; }
        public IDirectory Directory { get; private set; }
        public string AssemblyName { get; private set; }
        private XDocument _CsprojContent { get; set; }
        public XDocument CsprojContent
        {
            get
            {
                if (_CsprojContent == null)
                    _CsprojContent = XDocument.Load(CsprojFile.FullPath);
                return _CsprojContent;
            }
        }
        public IFile CsprojFile { get; private set; }


        private AssemblyNetFx48(IMediator mediator, IDirectory directory, string assemblyName)
        {
            Directory = directory;
            Mediator = mediator;
            AssemblyName = assemblyName;

        }
        public static async Task<AssemblyNetFx48> CreateAsync(IMediator mediator, IDirectory directory, string assemblyName)
        {
            AssemblyNetFx48 assemblyProject = new(mediator, directory, assemblyName);
            await assemblyProject.CreateCSprojFile();
            await mediator.Publish(new AssemblyAddedNotification(assemblyProject));
            return assemblyProject;
        }
        private async Task CreateCSprojFile()
        {
            var projectXml = new XDocument(
               new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                   new XElement("PropertyGroup",
                       new XElement("TargetFramework", "net48"),
                       new XElement("AssemblyName", AssemblyName)
                   )
               )
           );

            string csprojFileName = $"{AssemblyName}.csproj";

            using (MemoryStream stream = new MemoryStream())
            {
                projectXml.Save(stream);
                CsprojFile = await Directory.AddFileAsync(csprojFileName, stream);
            }
        }

        /// <summary>
        /// اضافه کردن کلاس جدید با namespace به صورت ساختاری و امن با Roslyn
        /// </summary>




    }
}
