using System.IO.Compression;

namespace ZipArchiveExample
{
    public class Document
    {
        public Document(int id, string name, bool isFolder, int? parentId = null)
        {
            Id = id;
            Name = name;
            IsFolder = isFolder;
            ParentId = parentId;
        }
        public int Id { get; private set; }
        public int? ParentId { get; private set; }
        public string Name { get; private set; }
        public bool IsFolder { get; private set; }        
    }
    
    public class Zipper
    {
        public List<Document> Documents
        {
            get
            {
                List<Document> list = new List<Document>();
                
                list.Add(new Document(1, $"SelectedFolder", true));
                list.Add(new Document(2, $"Memorandum.docx", false, 1));
                list.Add(new Document(3, $"Articles.pdf", false, 1));
                list.Add(new Document(4, $"Accounts", true, 1));
                list.Add(new Document(5, $"Readme.txt", false, 4));
                list.Add(new Document(6, $"2017", true, 4));
                list.Add(new Document(7, $"Final Accounts.xlsx", false, 6));
                list.Add(new Document(8, $"Draft Accounts.xlsx", false, 6));
                list.Add(new Document(9, $"2018", true, 4));
                list.Add(new Document(12, $"Final Accounts.xlsx", false, 9));
                list.Add(new Document(13, $"Draft Accounts.xlsx", false, 9));
                list.Add(new Document(10, $"2019", true, 4));
                list.Add(new Document(14, $"Final Accounts.xlsx", false, 10));
                list.Add(new Document(15, $"Draft Accounts.xlsx", false, 10));
                list.Add(new Document(11, $"2020", true, 4));
                list.Add(new Document(16, $"Final Accounts.xlsx", false, 11));
                list.Add(new Document(17, $"Draft Accounts.xlsx", false, 11));

                return list;
            }
        }
        public void Create()
        {
            using (MemoryStream ms = new MemoryStream())
            {

                using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create))
                {
                    AddEntry(archive, Documents.First(), null);
                }

                using (FileStream fs = new FileStream($"C:\\temp\\{Documents.First().Name}.zip", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(ms.ToArray());
                }                    
            }
        }
        private void AddEntry(ZipArchive archive, Document doc, string? parentPath = null)
        {
            if (doc.IsFolder)
            {
                foreach(var docu in Documents.Where(x => x.ParentId == doc.Id))
                {
                    AddEntry(archive, docu, $"{parentPath}{doc.Name}\\");
                }                
            }
            else
            {
                ZipArchiveEntry readmeEntry = archive.CreateEntry($"{parentPath}{doc.Name}");
                using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                {
                    writer.WriteLine($"Information about {doc.Name}.");
                    writer.WriteLine("===============================");
                }
            }


        }
    }
}
