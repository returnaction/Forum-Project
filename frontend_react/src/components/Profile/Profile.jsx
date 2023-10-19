import dayjs from "dayjs";
import React, { useState, useEffect } from "react";
import useAuth from "../../hooks/useAuth";
import axios from "axios";

const Profile = (userObj) => {
  const shortDateFormat = dayjs(userObj.userObj.registrationDate).format(
    "MM/DD/YYYY"
  );
  const [user, token] = useAuth();
  const [isEditing, setIsEditing] = useState(false);

  const [firstName, setFirstName] = useState();
  const [lastName, setLastName] = useState();
  const [email, setEmail] = useState();
  const [imageData, setImageData] = useState();

  var checkProfileIsAuthorizedUser = user.id === userObj.userObj.id;

  const handleUpdateButton = async (e) => {
    e.preventDefault();
    setIsEditing(!isEditing);
    setFirstName(userObj.userObj.firstName);
    setLastName(userObj.userObj.lastName);
    setEmail(userObj.userObj.email);
  };

  const updateUser = {
    firstName,
    lastName,
    email,
    file: imageData,
  };

  const formData = new FormData();
  formData.append("file", imageData);
  formData.append("firstName", firstName);
  formData.append("lastName", lastName);
  formData.append("email", email);

  function handleImageChange(e) {
    setImageData(e.target.files[0]);
    console.log(e.target.files);
  }
  // update profile
  const handleUpdateProfile = async (e) => {
    try {
      e.preventDefault();
      const response = await axios.put(
        `https://localhost:5001/api/user/`,
        formData,
        {
          headers: {
            Authorization: "Bearer " + token,
          },
        }
      );
      setIsEditing(!isEditing);
    } catch (error) {
      console.warn("Error in handleUpdateProfile method, ProfilePage", error);
    }
  };

  return (
    <div>
      <div className="profile-container">
        <div className="profile image">
          <img
            src={`data:image/jpeg;base64, ${userObj.userObj.profilePictureB64Base}`}
          />
        </div>

        <div className="profile information">
          <h5>Username: {userObj.userObj.userName}</h5>
          <h5>First Name: {userObj.userObj.firstName}</h5>
          <h5>Lastname: {userObj.userObj.lastName}</h5>
          <h5>Email: {userObj.userObj.email}</h5>
          <p>Registered: {shortDateFormat}</p>
        </div>

        <div className="profile-container">
          {isEditing ? (
            <div>
              <form className="update-form" onSubmit={handleUpdateProfile}>
                <div>
                  <label>FirtName</label>
                  <br />
                  <input
                    type="text"
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                  ></input>
                  <br />
                  <label>Lastname</label>
                  <br />
                  <input
                    type="text"
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                  ></input>
                  <br />
                  <label>Email</label>
                  <br />
                  <input
                    type="text"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                  ></input>
                  <br />
                  <label>Picture</label>
                  <br />
                  <input type="file" onChange={handleImageChange} />
                  <br />
                </div>

                <div>
                  <button className="button-new" type="submit">
                    Save Changes
                  </button>
                </div>
              </form>
              <div></div>
            </div>
          ) : null}
        </div>

        <div className="profile information">
          {checkProfileIsAuthorizedUser && !isEditing ? (
            <button className="button-new" onClick={handleUpdateButton}>
              Update Profile
            </button>
          ) : null}
        </div>
      </div>
    </div>
  );
};

export default Profile;
