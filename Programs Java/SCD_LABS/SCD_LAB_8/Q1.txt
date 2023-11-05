import java.io.*;
import java.util.ArrayList;
import java.util.List;

class File implements Serializable {
	private static final long serialVersionUID = 1L;
	private static int count = 0;
	private int fileId;
	private String fileName;
	private long size;
	private String owner;
	private byte[] content;

	public File(String fileName, String owner, byte[] content) {
		this.fileId = count++;
		this.fileName = fileName;
		this.size = content.length;
		this.owner = owner;
		this.content = content;
	}

	public int getFileId() {
		return fileId;
	}

	public String getFileName() {
		return fileName;
	}

	public long getSize() {
		return size;
	}

	public String getOwner() {
		return owner;
	}

	public byte[] getContent() {
		return content;
	}

	public static void serializeToFile(File file) {
		try {
			FileOutputStream fileOut = new FileOutputStream("files/" + file.getFileId() + ".dat");
			ObjectOutputStream out = new ObjectOutputStream(fileOut);
			out.writeObject(file);
			out.close();
			fileOut.close();
			System.out.println("Serialized data is saved in files/" + file.getFileId() + ".dat");
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	public static File deserializeFromFile(String filePath) {
		File file = null;
		try {
			FileInputStream fileIn = new FileInputStream(filePath);
			ObjectInputStream in = new ObjectInputStream(fileIn);
			file = (File) in.readObject();
			in.close();
			fileIn.close();
		} catch (IOException | ClassNotFoundException e) {
			e.printStackTrace();
		}
		return file;
	}

	@Override
	public String toString() {
		return "File ID: " + fileId +
				"\nFile Name: " + fileName +
				"\nSize: " + size +
				"\nOwner: " + owner;
	}
}

class FileManager {
	private List<File> fileList;

	public FileManager() {
		this.fileList = new ArrayList<>();
	}

	public void addFile(String fileName, String owner, byte[] content) {
		for (File file : fileList) {
			if (file.getFileName().equals(fileName)) {
				System.out.println("File with the same name already exists.");
				return;
			}
		}
		File newFile = new File(fileName, owner, content);
		fileList.add(newFile);
		File.serializeToFile(newFile);
	}

	public void viewFiles() {
		for (File file : fileList) {
			System.out.println(file.toString());
			System.out.println();
		}
	}
}

public class Main {
	public static void main(String[] args) {
		FileManager fileManager = new FileManager();
		byte[] content1 = "This is the content of file 1".getBytes();
		byte[] content2 = "This is the content of file 2".getBytes();

		fileManager.addFile("file1", "Ahmad Adnan", content1);
		fileManager.addFile("file2", "Anas Asim", content2);
		System.out.println();

		System.out.println("List of Files:");
		fileManager.viewFiles();
	}
}
