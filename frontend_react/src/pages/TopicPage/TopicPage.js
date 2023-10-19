import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";

//components
import Topic from "../../components/Topic/Topic";
import CommentsTable from "../../components/Topic/CommentsTable";
import AddComment from "../../components/Topic/AddComment";

const TopicPage = () => {
  const { topicId } = useParams();
  const [topicItem, setTopicItem] = useState(null);
  const [text, setText] = useState("");

  useEffect(() => {
    fetchTopic();
  }, [topicItem]);

  const fetchTopic = async () => {
    try {
      let response = await axios.get(
        `https://localhost:5001/api/topic/${topicId}`
      );
      setTopicItem(response.data);
    } catch (error) {
      console.log("Error in fetchTopic by id, in TopicPage profile ", error);
    }
  };

  return (
    <div className="profile-page">
      <div>{topicItem && <Topic topicItem={topicItem} />}</div>
      <div>
        {topicItem && <CommentsTable topicReviews={topicItem.comments} />}
      </div>
      <div className="profile comment-div">
        <AddComment text={text} setText={setText} topicId={topicId} />
      </div>
    </div>
  );
};

export default TopicPage;
