import axios from "axios";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import useAuth from "../../hooks/useAuth";

//components
import Profile from "../../components/Profile/Profile";
import TopicTable from "../../components/TopicTable/TopicTable";
import CommentsTable from "../../components/Topic/CommentsTable";
import SendMessage from "../../components/Profile/SendMessage";

const ProfilePage = () => {
  const { id } = useParams();
  const [userObj, setUserObj] = useState({});
  const [user, token] = useAuth();

  useEffect(() => {
    fetchUser();
  }, [userObj]);

  //get user information
  const fetchUser = async () => {
    try {
      let response = await axios.get(`https://localhost:5001/api/user/${id}`);
      setUserObj(response.data);
    } catch (error) {
      console.log("Error in fetchUser by Id, Profile Page", error);
    }
  };

  return (
    <div className="profile-page">
      <div className="profile-chat">
        <Profile userObj={userObj} />
        <SendMessage userObj={userObj} />
      </div>
      <h3 className="profile-titles">Your topics</h3>
      <TopicTable topics={userObj.topics} isProfilePage={true} />
      <h3 className="profile-titles">Your comments</h3>
      <CommentsTable topicReviews={userObj.comments} isProfilePage={true} />
    </div>
  );
};

export default ProfilePage;
