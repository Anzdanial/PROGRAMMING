package DAO;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.sql.*;
import java.util.HashMap;

public class MatchFileDAO {

    public MatchFileDAO(){

    }
    public void saveExactMatch() throws SQLException{
        try{
            BufferedWriter writer = new BufferedWriter(new FileWriter("exactMatch.txt"));
            String query = "Select taskName, resourceName from taskmatch;";
            Connection connection = getConnection();
            PreparedStatement statement = connection.prepareStatement(query);
            ResultSet rs = statement.executeQuery();
            String prev = "";
            boolean flag = false;
            while(rs.next()){
                if(rs.getString(1).equals(prev)){
                    String resourceName = rs.getString(2);
                    writer.write(", "+resourceName);
                    flag = true;
                }
                else{
                    if(flag){
                        writer.newLine();
                    }
                    String taskName = rs.getString(1);
                    prev = taskName;
                    String resourceName = rs.getString(2);
                    writer.write(taskName+" | ");
                    writer.write(resourceName);
                    flag = true;
                }
            }
            writer.close();
        }
        catch (IOException e1){
            e1.printStackTrace();
        }
    }
    public void saveSkillOnlyMatch() throws SQLException{
        try{
            BufferedWriter writer = new BufferedWriter(new FileWriter("skillOnlyMatch.txt"));
            String query = "Select taskName, resourceName from taskmatchskill;";
            Connection connection = getConnection();
            PreparedStatement statement = connection.prepareStatement(query);
            ResultSet rs = statement.executeQuery();
            String prev = "";
            boolean flag = false;
            while(rs.next()){
                if(rs.getString(1).equals(prev)){
                    String resourceName = rs.getString(2);
                    writer.write(", "+resourceName);
                    flag = true;
                }
                else{
                    if(flag){
                        writer.newLine();
                    }
                    String taskName = rs.getString(1);
                    prev = taskName;
                    String resourceName = rs.getString(2);
                    writer.write(taskName+" | ");
                    writer.write(resourceName);
                    flag = true;
                }
            }
            writer.close();
        }
        catch (IOException e1){
            e1.printStackTrace();
        }
    }
//    public HashMap<String,String> loadMatchResults() throws SQLException {
//        HashMap<String,String> results = new HashMap<>();
//        String query = "Select taskName, resourceName from taskmatch;";
//        Connection connection = getConnection();
//        PreparedStatement statement = connection.prepareStatement(query);
//        ResultSet rs = statement.executeQuery();
//        while(rs.next()){
//            String taskName = rs.getString(1);
//            String resourceName = rs.getString(2);
//            results.put(taskName,resourceName);
//        }
//        return results;
//    }
    private static Connection getConnection() throws SQLException {
        return DriverManager.getConnection("jdbc:mysql://localhost:3307/resourceallocation","root","");
    }
}
