import axios from "axios";
import useAuth from "../../hooks/useAuth";
import React, { useState, useEffect } from "react";

//components
import ListOfUsers from "../../components/DirectMessages/ListOfUsers";
import DirectMessagesUser from "../../components/DirectMessages/DirectMessagesUser";

const DirectMessagesPage = () => {
  const [user, token] = useAuth();
  const [users, setUsers] = useState([]);
  const [messages, setMessages] = useState([]);
  const [activeIndex, setActiveIndex] = useState(-1);

  useEffect(() => {
    fetchUsers();
  }, []);

  useEffect(() => {
    fetchMessages();
  }, [activeIndex]);

  const fetchUsers = async () => {
    try {
      let response = await axios.get(
        `https://localhost:5001/api/directmessage`,
        {
          headers: {
            Authorization: "Bearer " + token,
          },
        }
      );
      setUsers(response.data);
    } catch (error) {
      console.log("Error in MessagesPage ", error);
    }
  };

  const fetchMessages = async () => {
    try {
      if (activeIndex === -1) {
        return;
      }

      let response = await axios.get(
        `https://localhost:5001/api/directmessage/messages/${users[activeIndex].id}`,
        {
          headers: {
            Authorization: "Bearer " + token,
          },
        }
      );
      console.log(response.data);
      setMessages(response.data);
    } catch (error) {
      console.log("Error in FetchMessages in Messages Page", error);
    }
  };

  return (
    <div className="direct-message-page-div">
      <div className="listofUsers">
        <ListOfUsers
          users={users}
          activeIndex={activeIndex}
          setActiveIndex={setActiveIndex}
        />
      </div>
      <div>{messages && <DirectMessagesUser messages={messages} />}</div>
    </div>
  );
};

export default DirectMessagesPage;
