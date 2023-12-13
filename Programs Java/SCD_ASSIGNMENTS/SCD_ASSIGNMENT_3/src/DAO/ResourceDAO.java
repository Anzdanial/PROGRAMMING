package DAO;

import Model.Resource;
import Model.Skill;

import java.sql.*;
import java.util.ArrayList;

public class ResourceDAO {
//    private Resource resource;
    public ResourceDAO(){
//        this.resource = r;
    }
    public boolean saveResource(Resource r) throws SQLException{
        int count = 0;
        Connection conn = getConnection();
        PreparedStatement stmt = updateStatementResource(conn,r);
        count = stmt.executeUpdate();
        if(count==0){
            stmt = insertStatementResource(conn,r);
            count = stmt.executeUpdate();
        }
        if(count>0){
            return true;
        }
        else{
            return false;
        }
    }
    public boolean updateResource(String oldName,String newName) throws SQLException{
        Connection connection = getConnection();
        String query = "update resource set name = ? where name = ?;";
        PreparedStatement statement = connection.prepareStatement(query);
        statement.setString(1,newName);
        statement.setString(2,oldName);
        int count = statement.executeUpdate();
        return count > 0 ? true : false;
    }
    public boolean updateSkill(Skill oldSkill,Skill newSkill,String rname) throws SQLException{
        Connection connection = getConnection();
        String query = "update resourceskills set type = ?, experience = ?  where type = ? AND resourceName = ?;";
        PreparedStatement statement = connection.prepareStatement(query);
        statement.setString(1,newSkill.getType());
        statement.setString(2,newSkill.getExperience());
        statement.setString(3,oldSkill.getType());
        statement.setString(4,rname);
        int count = statement.executeUpdate();
        return count > 0 ? true : false;
    }
    public boolean saveSkill(Skill s,String rname) throws SQLException{
        int count = 0;
        Connection conn = getConnection();
        PreparedStatement stmt = updateStatementSkills(conn,s,rname);
        count = stmt.executeUpdate();
        if(count==0){
            stmt = insertStatementSkills(conn,s,rname);
            count = stmt.executeUpdate();
        }
        if(count>0){
            return true;
        }
        else{
            return false;
        }
    }
    public ArrayList<Skill> loadSkills(String rname) throws SQLException{
        ArrayList<Skill> skills = new ArrayList<>();
        Connection conn = getConnection();
        String query = "Select type,experience from resource as r inner join resourceskills as rs ON r.name = rs.resourceName where rs.resourceName = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,rname);
        ResultSet rs = stmt.executeQuery();
        while (rs.next()){
            String type = rs.getString(1);
            String experience = rs.getString(2);
            Skill s = new Skill();
            s.setType(type);
            s.setExperience(experience);
            skills.add(s);
        }
        return skills;
    }
    public ArrayList<Resource> loadResources() throws SQLException{
        Connection conn = getConnection();
        String query = "Select name from resource;";
        PreparedStatement stmt = conn.prepareStatement(query);
        ResultSet rs = stmt.executeQuery();
        ArrayList<Resource> resources = new ArrayList<>();
        while(rs.next()){
            String name = rs.getString(1);
            Resource r = new Resource();
            r.setName(name);
            String query2 = "Select type,experience from resource as r inner join resourceskills as rs ON r.name = rs.resourceName where rs.resourceName = ?;";
            PreparedStatement stmt2 = conn.prepareStatement(query2);
            stmt2.setString(1,r.getName());
            ResultSet rs2 = stmt2.executeQuery();
            while (rs2.next()){
                String type = rs2.getString(1);
                String experience = rs2.getString(2);
                r.setSkills(type,experience);
            }
            resources.add(r);
        }
        return resources;
    }
    //added
    public ArrayList<Resource> loadResourcesNames() throws SQLException{
        Connection conn = getConnection();
        String query = "Select name from resource;";
        PreparedStatement stmt = conn.prepareStatement(query);
        ResultSet rs = stmt.executeQuery();
        ArrayList<Resource> resources = new ArrayList<>();
        while(rs.next()){
            String name = rs.getString(1);
            Resource r = new Resource();
            r.setName(name);
            resources.add(r);
        }
        return resources;
    }
    public boolean deleteResource(String name) throws SQLException{
        int count = 0;
        Connection conn = getConnection();
        PreparedStatement stmt = deleteStatementResource(conn,name);
        count = stmt.executeUpdate();
        if(count > 0){
            return true;
        }
        else{
            return false;
        }
    }
    public boolean deleteSkill(Skill s,String rname) throws SQLException{
        Connection connection = getConnection();
        String query  = "delete from resourceskills where resourceName = ? AND type = ?;";
        PreparedStatement statement = connection.prepareStatement(query);
        statement.setString(1,rname);
        statement.setString(2,s.getType());
        int count = statement.executeUpdate();
        return count > 0 ? true : false;
    }

    private PreparedStatement insertStatementResource(Connection conn,Resource r) throws SQLException {
        String query = "Insert into resource(name) values(?);";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,r.getName());
        return stmt;
    }
    private PreparedStatement insertStatementSkills(Connection conn, Skill s,String rname) throws SQLException {
        String query = "Insert into resourceskills(type,experience,resourceName) values(?,?,?);";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,s.getType());
        stmt.setString(2,s.getExperience());
        stmt.setString(3,rname);
        return stmt;
    }
    //this is for over-writing if already exists
    private PreparedStatement updateStatementSkills(Connection conn, Skill s,String rname) throws SQLException {
        String query = "update resourceskills set type = ?, experience = ?, resourceName = ? where type = ? AND resourceName = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,s.getType());
        stmt.setString(2,s.getExperience());
        stmt.setString(3,rname);
        stmt.setString(4,s.getType());
        stmt.setString(5,rname);
        return stmt;
    }
    private PreparedStatement updateStatementResource(Connection conn,Resource r) throws SQLException{
        String query = "Update resource set name = ? where name = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,r.getName());
        stmt.setString(2,r.getName());
        return stmt;
    }
    private PreparedStatement deleteStatementResource(Connection conn,String name) throws SQLException{
        String query = "delete from resource where name = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,name);
        return stmt;
    }
    private PreparedStatement deleteStatementSkill(Connection conn,String type,String rname) throws SQLException{
        String query = "delete from resourceskills where type = ? AND resourceName = ?;";
        PreparedStatement stmt = conn.prepareStatement(query);
        stmt.setString(1,type);
        stmt.setString(2,rname);
        return stmt;
    }
    public static Connection getConnection() throws SQLException {
        return DriverManager.getConnection("jdbc:mysql://localhost:3307/resourceallocation","root","");

    }


}
