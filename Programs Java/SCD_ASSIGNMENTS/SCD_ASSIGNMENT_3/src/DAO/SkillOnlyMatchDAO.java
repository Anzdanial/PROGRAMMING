package DAO;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.SQLException;

public class SkillOnlyMatchDAO {
    public SkillOnlyMatchDAO(){

    }
    public boolean saveTaskMatch(String tname,String rname) throws SQLException {
        int count = 0;
        Connection connection = getConnection();
        PreparedStatement statement = updateStatement(connection,tname,rname);
        count = statement.executeUpdate();
        if(count==0){
            statement = insertSatatement(connection,tname,rname);
            count = statement.executeUpdate();
        }
        return count > 0 ? true : false;
    }
    private PreparedStatement insertSatatement(Connection conn,String tname,String rname) throws SQLException{
        String query = "Insert into taskmatchskill(taskName,resourceName) values(?,?);";
        PreparedStatement statement = conn.prepareStatement(query);
        statement.setString(1,tname);
        statement.setString(2,rname);
        return statement;
    }
    private PreparedStatement updateStatement(Connection conn,String tname,String rname) throws SQLException{
        String query2 = "update taskmatchskill set taskName = ?, resourceName = ? where taskName = ? AND resourceName = ?;";
        PreparedStatement statement = conn.prepareStatement(query2);
        statement.setString(1,tname);
        statement.setString(2,rname);
        statement.setString(3,tname);
        statement.setString(4,rname);
        return statement;
    }
    private static Connection getConnection() throws SQLException {
        return DriverManager.getConnection("jdbc:mysql://localhost:3307/resourceallocation","root","");
    }


}
