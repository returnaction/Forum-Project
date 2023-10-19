import axios from "axios";
import useAuth from "../../hooks/useAuth";

const AddComment = ({ text, setText, topicId }) => {
  const [user, token] = useAuth();

  const handleAddComent = async (e) => {
    e.preventDefault();

    const formData = {
      text,
      topicId,
    };

    try {
      const response = await axios.post(
        "https://localhost:5001/api/comment",
        formData,
        {
          headers: {
            Authorization: "Bearer " + token,
          },
        }
      );
    } catch (error) {
      console.warn("Error submitting new comment in TopicPage", error);
    }
  };
  return (
    <div className="comment-div">
      <form onSubmit={handleAddComent}>
        <div>
          <div>
            <textarea
              rows="5"
              cols="60"
              onChange={(e) => setText(e.target.value)}
              placeholder="comment"
              className="comment-textarea"
            />
          </div>
          <button type="submit" className="button-new">
            Add
          </button>
        </div>
      </form>
    </div>
  );
};

export default AddComment;
