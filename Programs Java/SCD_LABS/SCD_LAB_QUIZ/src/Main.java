import java.io.FileReader;
import java.sql.Array;
import java.util.ArrayList;

class Marks{
	private Course course;
	private Student student;
	private int marks;
	public Course getCourse(){
		return course;
	}

	public Student getStudent(){
		return student;
	}
	public String getMarks(){
		return String.valueOf(marks);
	}
	public void setMarks(Student s, Course c, int Marks){
		student = s;
		course = c;
		marks = Marks;
	}
}

class Student{
	private int roll_Number;
	private String name;
	Student(int roll, String Name){
		roll_Number = roll;
		name = Name;
	}

	public Student() {

	}

	public void setRoll_Number(int num){
		roll_Number = num;
	}
	public int getRoll(){
		return roll_Number;
	}
	public String getName(){
		return name;
	}
}

class Course{
	Teacher teacher;
	private int courseID;
	private String name;
	private int teacherID;
	Course(){}
	Course(int ID, String Name, int cnicTeacher){
		name = Name;
		courseID = ID;
		teacherID = cnicTeacher;
	}

	public void setData(int ID, String Name, int cnicNumber){
		name = Name;
		courseID = ID;
		teacherID = cnicNumber;
	}
	public void setID(int ID){
		courseID = ID;
	}
	public void setName(String Name){
		name = Name;
	}
	public void setTeacherID(int cnicNumber){
		teacherID = cnicNumber;
	}
	public Teacher getAssignedTeacher(){
		return teacher;
	}

	public String getName(){
		return name;
	}

	public int getCourseID(){
		return courseID;
	}
}

class Teacher{
	private String name;
	private int cnicNumber;
	Teacher(String Name, int cnic){
		name = Name;
		cnicNumber = cnic;
	}
	public String getName(){
		return name;
	}
	public int getCNIC(){
		return cnicNumber;
	}
}


public class Main {
	static ArrayList<Marks> setMarks(){
		ArrayList <Marks> students = new ArrayList<>();
		char[] array = new char[1024];
		int characterRead = 0;
		try {
			FileReader input = new FileReader("marks.txt");
			characterRead = input.read(array);
			input.close();
		} catch (Exception e) {
			e.getStackTrace();
		}
		char[] arrayNew = new char[characterRead];
		for (int i = 0; i < characterRead; i++)
			arrayNew[i] = array[i];
		String inputString = new String(arrayNew);
		String [] result = inputString.split("\n");
		String [][]result2 = new String[result.length][];
		for(int i = 0; i < result.length; i++) {
			result2[i] = result[i].split(",");
		}
		Marks temp = new Marks();
		Student temp1 = new Student();
		Course temp2 = new Course();

		for(int i = 0; i < result2.length; i++){
			int j = 0;
			temp1.setRoll_Number(Integer.parseInt(result2[i][j]));
			temp2.setID(Integer.parseInt(result2[i][j+1]));
			temp.setMarks(temp1,temp2,Integer.parseInt(result2[i][j+2].trim()));
			students.add(temp);
		}
		return students;
	}
	static void calculateCourseGrade(Course courseID, ArrayList<Marks> students){
		for(int i = 0; i < students.size(); i++){
			if(students.get(i).getCourse() == courseID){
				String str = students.get(i).getMarks();
				int intValue = Integer.parseInt(str);
				if(intValue > 90)
					System.out.println(students.get(i).getStudent().getRoll() + "  " + students.get(i).getMarks() + "  " + "Grade is A");
				else if(intValue > 70)
					System.out.println(students.get(i).getStudent().getRoll() + "  " + students.get(i).getMarks() + "  " + "Grade is B");
				else if(intValue > 60)
					System.out.println(students.get(i).getStudent().getRoll() + "  " + students.get(i).getMarks() + "  " + "Grade is C");
				else if(intValue > 50)
					System.out.println(students.get(i).getStudent().getRoll() + "  " + students.get(i).getMarks() + "  " + "Grade is D");
				else if(intValue < 50)
					System.out.println(students.get(i).getStudent().getRoll() + "  " + students.get(i).getMarks() + "  " + "Grade is F");
			}
		}
	}

	static void searchStudentRollNo(ArrayList<Marks> list1, int roll){
		for(int i = 0; i < list1.size(); i++){
			if(roll == list1.get(i).getStudent().getRoll()){
				System.out.println("STUDENT FOUND!!");
				System.out.println(list1.get(i).getStudent().getName() + "  " + list1.get(i).getStudent().getRoll() + " " + list1.get(i).getCourse().getName() + "   " + list1.get(i).getCourse().getAssignedTeacher().getName());
			}
			else

		}
	}
	static ArrayList<Student> setStudents(){
		ArrayList <Student> students = new ArrayList<>();
		char[] array = new char[1024];
		int characterRead = 0;
		try {
			FileReader input = new FileReader("students.txt");
			characterRead = input.read(array);
			input.close();
		} catch (Exception e) {
			e.getStackTrace();
		}
		char[] arrayNew = new char[characterRead];
		for (int i = 0; i < characterRead; i++)
			arrayNew[i] = array[i];
		String inputString = new String(arrayNew);
		String [] result = inputString.split("\n");
		String [][]result2 = new String[result.length][];
		for(int i = 0; i < result.length; i++) {
			 result2[i] = result[i].split(",");
		}
		for(int i = 0; i < result2.length; i++){
			int j = 0;
			Student temp = new Student(Integer.parseInt(result2[i][j]),result2[i][j+1]);
			students.add(temp);
		}
		return students;
	}

	static ArrayList<Course> setCourses(){
		ArrayList <Course> courses = new ArrayList<>();
		char[] array = new char[1024];
		int characterRead = 0;
		try {
			FileReader input = new FileReader("courses.txt");
			characterRead = input.read(array);
			input.close();
		} catch (Exception e) {
			e.getStackTrace();
		}
		char[] arrayNew = new char[characterRead];
		for (int i = 0; i < characterRead; i++)
			arrayNew[i] = array[i];
		String inputString = new String(arrayNew);
		String [] result = inputString.split(",");
		for(int i = 0; i < result.length; i++){
			Course temp = new Course();
			if(i % 2 == 0) {
				System.out.println(result[i]);
				temp.setID(Integer.parseInt(result[i]));
				System.out.println(result[i + 2]);
				temp.setTeacherID(Integer.parseInt(result[i+2]));
			}
			else{
				System.out.println(result[i + 1]);
				temp.setName(result[i+1]);
			}
			courses.add(temp);
		}
		return courses;
	}

	static ArrayList<Teacher> setTeachers(){
		ArrayList <Teacher> teachers = new ArrayList<>();
		char[] array = new char[1024];
		int characterRead = 0;
		try {
			FileReader input = new FileReader("students.txt");
			characterRead = input.read(array);
			input.close();
		} catch (Exception e) {
			e.getStackTrace();
		}
		char[] arrayNew = new char[characterRead];
		for (int i = 0; i < characterRead; i++)
			arrayNew[i] = array[i];
		String inputString = new String(arrayNew);
		String [] result = inputString.split(",");
		for(int i = 0; i < result.length; i++){
			if(i % 2 != 0) {
				Teacher temp = new Teacher(result[i], Integer.parseInt(result[i + 1]));
				teachers.add(temp);
			}
		}
		return teachers;
	}

	public static void main(String[] args) {
		ArrayList <Student> students = setStudents();
		ArrayList <Marks> marks = setMarks();
		searchStudentRollNo(marks,1);

//		ArrayList <Course> courses = setCourses();
//		ArrayList <Teacher> teachers = setTeachers();
	}
}