import UserItem from "./UserItem";
const ListOfUsers = ({ users = [], activeIndex, setActiveIndex }) => {
  const userItem = users.map((user, index) => (
    <UserItem
      userObj={user}
      key={user.id}
      activeIndex={activeIndex}
      setActiveIndex={setActiveIndex}
      index={index}
    />
  ));
  return (
    <div>
      <h3 className="users-h3">Users</h3>
      <div>{userItem}</div>
    </div>
  );
};

export default ListOfUsers;
