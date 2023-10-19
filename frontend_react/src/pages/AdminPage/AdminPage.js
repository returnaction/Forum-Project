import { Chart } from "react-google-charts";
import axios from "axios";
import React, { useState, useEffect } from "react";
import useAuth from "../../hooks/useAuth";

export const options = {
  title: "Topics",
};

const AdminPage = () => {
  const [user, token] = useAuth();
  const [topics, setTopics] = useState([]);
  const [users, setUsers] = useState([]);
  const [comments, setComments] = useState([]);

  console.log(user);

  useEffect(() => {
    fetchTopics();
    fetchUsers();
    fetchСomments();
  }, []);

  //topics
  const topicsByDate = topics.reduce((result, topic) => {
    const byDate = result[topic.timePosted];

    return {
      ...result,
      [topic.timePosted]: byDate ? [...byDate, topic] : [topic],
    };
  }, {});

  const data = [
    ["Date", "Topics"],
    ...Object.keys(topicsByDate).map((date) => {
      console.log(date);
      return [date, topicsByDate[date].length];
    }),
  ];
  console.log(data);

  // get all topics
  const fetchTopics = async () => {
    try {
      let response = await axios.get(`https://localhost:5001/api/topic`);
      setTopics(response.data);
    } catch (error) {
      console.warn("Error in fetchTopics method , AdminPage", error);
    }
  };

  //users
  const usersByDate = users.reduce((result, user) => {
    const byDate = result[user.registrationDate];

    return {
      ...result,
      [user.registrationDate]: byDate ? [...byDate, user] : [user],
    };
  }, {});

  const dataUser = [
    ["Date", "Users"],
    ...Object.keys(usersByDate).map((date) => {
      console.log(date);
      return [date, usersByDate[date].length];
    }),
  ];

  const fetchUsers = async () => {
    try {
      let resonse = await axios.get(`https://localhost:5001/api/user`);
      setUsers(resonse.data);
    } catch (error) {
      console.warn("Error in fetchUsers method, Admin Page", error);
    }
  };

  // comments

  const commentsByDate = comments.reduce((result, comment) => {
    const byDate = result[comment.timePosted];

    return {
      ...result,
      [comment.timePosted]: byDate ? [...byDate, comment] : [comment],
    };
  }, {});

  const dataComments = [
    ["Date", "Comments"],
    ...Object.keys(commentsByDate).map((date) => {
      console.log(date);
      return [date, commentsByDate[date].length];
    }),
  ];

  const fetchСomments = async () => {
    try {
      let resonse = await axios.get(`https://localhost:5001/api/comment`);
      setComments(resonse.data);
    } catch (error) {
      console.warn("Error in fetchComments method, Admin Page", error);
    }
  };

  return user.isAdmin === "True" ? (
    <div className="admindiv">
      <h3 className="chart">Topics created daily:</h3>
      <Chart
        chartType="Bar"
        width="100%"
        height="400px"
        data={data}
        options={options}
      />
      <h3 className="chart">Users registered daily</h3>
      <Chart
        chartType="Bar"
        width="100%"
        height="400px"
        data={dataUser}
        options={options}
      />
      <h3 className="chart">Comments left daily</h3>
      <Chart
        chartType="Bar"
        width="100%"
        height="400px"
        data={dataComments}
        options={options}
      />
    </div>
  ) : (
    <div>Not Allowed</div>
  );
};

export default AdminPage;
