import dayjs from "dayjs";
import { useEffect, useState } from "react";
import useAuth from "../../hooks/useAuth";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.css";

const Topic = ({ topicItem }) => {
  const [user, token] = useAuth();
  const [isEditing, setIsEditing] = useState(false);
  const [text, setText] = useState();
  const [title, setTitle] = useState();
  const navigate = useNavigate();

  var checkProfileIsAuthorizedUser = user.id === topicItem.authorOfTopic.id;

  const handleUpdateButton = async (e) => {
    e.preventDefault();
    setIsEditing(!isEditing);
    setText(topicItem.text);
    setTitle(topicItem.title);
  };

  const updatedTopic = {
    title,
    text,
  };

  console.log(topicItem);

  // update topic
  const handleUpdateTopic = async (e) => {
    try {
      e.preventDefault();
      const response = await axios.put(
        `https://localhost:5001/api/topic/${topicItem.topicId}`,
        updatedTopic,
        {
          headers: {
            Authorization: "Bearer " + token,
          },
        }
      );
      setIsEditing(!isEditing);
    } catch (error) {
      console.warn("Error in Topic Page , Update Button", error);
    }
  };

  // delete topic
  const handleDeleteTopic = async (e) => {
    try {
      const response = await axios.delete(
        `https://localhost:5001/api/topic/${topicItem.topicId}`,
        {
          headers: {
            Authorization: "Bearer " + token,
          },
        }
      );
      navigate("/");
    } catch (error) {
      console.warn("Error in Home Page , Topic Item, Delete Button", error);
    }
  };

  //date format
  const shortDateFormat = dayjs(topicItem.timePosted).format("MM/DD/YYYY");

  return (
    <div>
      <div className="topic-page-profile profile-container">
        <div className="profile-info-topic">
          <img
            src={`data:image/jpeg;base64, ${topicItem.authorOfTopic.profilePictureB64Base}`}
          />
        </div>
        <div className="profile information">
          <h5>Author: {topicItem.authorOfTopic.userName}</h5>
          <h5>Likes: {topicItem.likes}</h5>
          <p>Published: {shortDateFormat}</p>
        </div>
      </div>

      {isEditing ? (
        <div>
          <form onSubmit={handleUpdateTopic}>
            <div className="titleOfTopic">
              <br />
              <label>Title</label>
            </div>
            <input
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
            ></input>
            <br />
            <br />
            <label>Text</label>
            <br />
            <textarea
              className="topic-textarea"
              value={text}
              onChange={(e) => setText(e.target.value)}
              rows="6"
              cols="175"
            ></textarea>
            <br />
            <div>
              <button type="submit" className="button-new">
                Save
              </button>
            </div>
          </form>
        </div>
      ) : (
        <div className="topic-container-div">
          <h3>{topicItem.title}</h3>
          <p> {topicItem.text}</p>
        </div>
      )}

      <div className="profile-update-delete">
        <div>
          {checkProfileIsAuthorizedUser && !isEditing ? (
            <button onClick={handleUpdateButton} className="button-new">
              Update
            </button>
          ) : null}
        </div>

        <div>
          {checkProfileIsAuthorizedUser ? (
            <button onClick={handleDeleteTopic} className="button-new">
              Delete
            </button>
          ) : null}
        </div>
      </div>
    </div>
  );
};

export default Topic;
