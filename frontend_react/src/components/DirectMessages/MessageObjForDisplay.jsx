import "./MessageObjForDisplay.css";
import dayjs from "dayjs";
import useAuth from "../../hooks/useAuth";

const MessageObjForDisplay = ({ message }) => {
  const [user, token] = useAuth();

  const shortDateFormat = dayjs(message.messageTime).format("MM/DD/YYYY HH:mm");
  console.log(message);
  console.log(user);

  var isMyMessage = message.fromUserId == user.id ? true : false;

  return (
    <div>
      {isMyMessage ? (
        <div className="containerChat">
          <img
            className="right"
            src={`data:image/jpeg;base64, ${message.fromUser.profilePictureB64Base}`}
          />
          <p>{message.text}</p>
          <span className="time-right">{shortDateFormat}</span>
        </div>
      ) : (
        <div className="containerChat darker">
          <img
            src={`data:image/jpeg;base64, ${message.fromUser.profilePictureB64Base}`}
          />
          <p>{message.text}</p>
          <span className="time-left">{shortDateFormat}</span>
        </div>
      )}
    </div>
  );
};

export default MessageObjForDisplay;

{
  /* <div>
        <h4>{message.fromUser.userName}</h4>
      </div>
      <div>
        <p>{message.text}</p>
      </div>
      <span>{shortDateFormat}</span> */
}

{
  /* <img src="/w3images/avatar_g2.jpg" alt="Avatar" className="right" /> */
}
