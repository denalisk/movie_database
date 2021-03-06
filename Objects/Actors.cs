using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace Catalog.Objects
{
  public class Actor
  {
    private string _name;
    private int _id;

    public Actor(string newName, int newId=0)
    {
      _name = newName;
      _id = newId;
    }

    public static void DeleteAll()
    {
      SqlConnection connection = DB.Connection();
      connection.Open();

      SqlCommand command = new SqlCommand("DELETE FROM actors;", connection);
      command.ExecuteNonQuery();
      connection.Close();
    }

    public static List<Actor> GetAll()
    {
      List<Actor> allActors = new List<Actor>{};
      SqlConnection connection = DB.Connection();
      connection.Open();

      SqlCommand command = new SqlCommand("SELECT * FROM actors;", connection);
      SqlDataReader reader = command.ExecuteReader();

      while(reader.Read())
      {
        int actorId = reader.GetInt32(0);
        string actorName = reader.GetString(1);
        Actor newActor = new Actor(actorName, actorId);
        allActors.Add(newActor);
      }
      return allActors;
    }

    public void Save()
    {
      SqlConnection connection = DB.Connection();
      connection.Open();

      SqlCommand command = new SqlCommand("INSERT INTO actors (name) OUTPUT INSERTED.id VALUES (@ActorName);", connection);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@ActorName";
      nameParameter.Value = this.GetName();
      command.Parameters.Add(nameParameter);

      SqlDataReader reader = command.ExecuteReader();

      while(reader.Read())
      {
        this._id = reader.GetInt32(0);
      }
      if (reader != null)
      {
        reader.Close();
      }
      if (connection != null)
      {
        connection.Close();
      }
    }

    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }



  }
}
