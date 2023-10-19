import React from "react";
import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

//components
import TopicTable from "../../components/TopicTable/TopicTable";

const HomePage = ({}) => {
  const [topics, setTopics] = useState([]);

  useEffect(() => {
    fetchTopic();
  }, [topics]);

  // get all topics
  const fetchTopic = async () => {
    try {
      let response = await axios.get(`https://localhost:5001/api/topic`);
      setTopics(response.data);
    } catch (error) {
      console.log("Error in fetchTopics method, HomePage", error);
    }
  };

  return (
    <div className="home-page">
      <div className="d-flex justify-content-center">
        <Link to="/topic">
          <button className="button-new" type="button">
            New Topic
          </button>
        </Link>
      </div>
      <TopicTable topics={topics} isProfilePage={false} />
    </div>
  );
};

export default HomePage;
