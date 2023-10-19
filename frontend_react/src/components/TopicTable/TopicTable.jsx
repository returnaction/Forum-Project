import TopicItem from "./TopicItem";

const TopicTable = ({ topics = [], isProfilePage }) => {
  const topicItem = topics.map((topic) => (
    <TopicItem
      key={topic.topicId}
      topicObject={topic}
      isProfilePage={isProfilePage}
    />
  ));

  return (
    topicItem && (
      <table className="table">
        <thead>
          <tr>
            <th>Id</th>
            <th>Title</th>

            <th>Author</th>
            <th>Published</th>
            <th>Edited</th>
            <th>Likes</th>
          </tr>
        </thead>
        <tbody>{topicItem}</tbody>
      </table>
    )
  );
};

export default TopicTable;
