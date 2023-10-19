import MessageObjForDisplay from "./MessageObjForDisplay";
const DirectMessagesUser = ({ messages }) => {
  const messageItem = messages.map((message, index) => (
    <MessageObjForDisplay message={message} key={index} />
  ));
  return (
    <div>
      <h3 className="messages-h3">Messages</h3>
      <div>{messageItem}</div>
    </div>
  );
};

export default DirectMessagesUser;
