import "./MessageObjForDisplay.css";

const UserItem = ({ userObj, activeIndex, setActiveIndex, index }) => {
  const handleActive = () => {
    setActiveIndex(index);
  };
  return (
    <div className="usernamedivchat">
      <div onClick={handleActive}>
        <h4>{userObj.userName}</h4>
      </div>
    </div>
  );
};

export default UserItem;
