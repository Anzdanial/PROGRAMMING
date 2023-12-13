package DAO;

import Model.Resource;
import Model.Skill;
import Model.Task;

import java.sql.*;
import java.util.ArrayList;

public class TaskDAO {
    public TaskDAO(){

    }
    public ArrayList<Skill> loadSkills(String tname) throws SQLException{
        ArrayList<Skill> skills = new ArrayList<>();
        Connection conn = getConnection();
        //should load respective skills
        String query = "Select type, experience from task as t inner join taskskills as ts ON t.name = ts.taskName where ts.taskName = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,tname);
        ResultSet rs = stmt.executeQuery();
        while(rs.next()){
            String type = rs.getString(1);
            String experience = rs.getString(2);
            Skill s = new Skill();
            s.setType(type);
            s.setExperience(experience);
            skills.add(s);
        }
        return skills;
    }
    public ArrayList<Task> loadTasks() throws SQLException{
        Connection conn = getConnection();
        String query = "Select name from task;";
        PreparedStatement stmt = conn.prepareStatement(query);
        ResultSet rs = stmt.executeQuery();
        ArrayList<Task> tasks = new ArrayList<>();
        while(rs.next()){
            String name = rs.getString(1);
            Task t = new Task();
            t.setName(name);
            String query2 = "Select type,experience from task as t Inner Join taskskills as ts ON t.name = ts.taskName where taskName = ?;";
            PreparedStatement stmt2 = conn.prepareStatement(query2);
            stmt2.setString(1,t.getName());
            ResultSet rs2 = stmt2.executeQuery();
            while(rs2.next()){
                String type = rs2.getString(1);
                String experience = rs2.getString(2);
                t.setTaskSkills(type,experience);
            }
            tasks.add(t);
        }
        return tasks;
    }
    //added
    public ArrayList<Task> loadTaskNames() throws SQLException{
        Connection conn = getConnection();
        String query = "Select name from task;";
        PreparedStatement stmt = conn.prepareStatement(query);
        ResultSet rs = stmt.executeQuery();
        ArrayList<Task> tasks = new ArrayList<>();
        while(rs.next()){
            String name = rs.getString(1);
            Task t = new Task();
            t.setName(name);
            tasks.add(t);
        }
        return tasks;
    }
    public boolean saveTask(Task t) throws SQLException{
        int count = 0;
        Connection conn = getConnection();
        PreparedStatement stmt = updateTaskStatement(conn,t);
        count = stmt.executeUpdate();
        if(count==0){
            stmt = insertTaskStatement(conn,t);
            count = stmt.executeUpdate();
        }
        return count > 0 ? true : false;
    }
    public boolean saveSkill(Skill s,String tname) throws SQLException{
        int count = 0;
        Connection conn = getConnection();
        PreparedStatement stmt = updateSkillStatement(conn,s,tname);
        count = stmt.executeUpdate();
        if(count==0){
            stmt = insertSkillStatement(conn,s,tname);
            count = stmt.executeUpdate();
        }
        return count > 0 ? true : false;
    }
    public boolean deleteTask(String name) throws SQLException{
        int count = 0;
        Connection conn = getConnection();
        PreparedStatement stmt = deleteTaskStatement(conn,name);
        count = stmt.executeUpdate();
        return count > 0 ? true : false;
    }
    public boolean updateTask(String oldName,String newName) throws SQLException{
        Connection connection = getConnection();
        String query = "update task set name = ? where name = ?;";
        PreparedStatement statement = connection.prepareStatement(query);
        statement.setString(1,newName);
        statement.setString(2,oldName);
        int count = statement.executeUpdate();
        return count > 0 ? true : false;
    }
    public boolean deleteSkill(String type,String tname) throws SQLException{
        int count = 0;
        Connection conn = getConnection();
        PreparedStatement stmt = deleteSkillStatement(conn,type,tname);
        count = stmt.executeUpdate();
        return count > 0 ? true : false;
    }
    public boolean updateSkill(Skill oldSkill,Skill newSKill,String tname) throws SQLException{
        Connection connection = getConnection();
        String query = "update taskskills set type = ?,experience = ? where taskName = ? AND type = ?;";
        PreparedStatement statement = connection.prepareStatement(query);
        statement.setString(1,newSKill.getType());
        statement.setString(2,newSKill.getExperience());
        statement.setString(3,tname);
        statement.setString(4,oldSkill.getType());
        int count = statement.executeUpdate();
        return count > 0 ? true : false;
    }
    private PreparedStatement insertTaskStatement(Connection conn, Task t) throws SQLException {
        String query = "Insert into task(name) values(?);";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,t.getName());
        return stmt;
    }
    private PreparedStatement insertSkillStatement(Connection conn, Skill s,String tname) throws SQLException {
        String query = "Insert into taskskills(type,experience,taskName) values(?,?,?);";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,s.getType());
        stmt.setString(2,s.getExperience());
        stmt.setString(3,tname);
        return stmt;
    }
    private PreparedStatement updateSkillStatement(Connection conn, Skill s,String tname) throws SQLException {
        String query = "update taskskills set type = ?, experience = ?, taskName = ? where type = ? AND taskName = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,s.getType());
        stmt.setString(2,s.getExperience());
        stmt.setString(3,tname);
        stmt.setString(4,s.getType());
        stmt.setString(5,tname);
        return stmt;
    }
    private PreparedStatement updateTaskStatement(Connection conn, Task t) throws SQLException {
        String query = "update task set name = ? where name = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,t.getName());
        stmt.setString(2,t.getName());
        return stmt;
    }
    private PreparedStatement deleteTaskStatement(Connection conn,String name) throws SQLException{
        String query = "delete from task where name = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,name);
        return stmt;
    }
    private PreparedStatement deleteSkillStatement(Connection conn,String type,String tname) throws SQLException{
        String query = "delete from taskskills where type = ? AND taskname = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,type);
        stmt.setString(2,tname);
        return stmt;
    }
    public static Connection getConnection() throws SQLException{
        return DriverManager.getConnection("jdbc:mysql://localhost:3307/resourceallocation","root","");
    }

}
