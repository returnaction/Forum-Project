import React from "react";
import { useContext } from "react";
import { useNavigate, Link } from "react-router-dom";
import AuthContext from "../../context/AuthContext";
import useAuth from "../../hooks/useAuth";

import "./NavBar.css";

const Navbar = () => {
  const { logoutUser, user } = useContext(AuthContext);
  const [userInfo, token] = useAuth();

  var isAdminResult = false;
  userInfo && (isAdminResult = userInfo.isAdmin === "True");

  const navigate = useNavigate();
  return (
    <ul>
      <li>
        <a className="navbar-a" onClick={() => navigate("/")}>
          Home
        </a>
      </li>
      <li>
        <a className="navbar-a" onClick={() => navigate(`/profile/${user.id}`)}>
          Profile
        </a>
      </li>
      <li>
        <a className="navbar-a" onClick={() => navigate("/messages")}>
          Messages
        </a>
      </li>
      {isAdminResult ? (
        <li>
          <a className="navbar-a" onClick={() => navigate("/admin")}>
            Admin
          </a>
        </li>
      ) : null}

      <li>
        {user ? (
          <a className="navbar-a" onClick={logoutUser}>
            Logout
          </a>
        ) : (
          <a className="navbar-a" onClick={() => navigate("/login")}>
            Login
          </a>
        )}
      </li>
    </ul>
  );
};

export default Navbar;

// <div className="">
//       <div>
//         <button
//           onClick={() => navigate("/")}
//           className="navbar-toggler whitebite"
//           type="button"
//           data-bs-toggle="collapse"
//           data-bs-target="#navbarNavAltMarkup"
//           aria-controls="navbarNavAltMarkup"
//           aria-expanded="false"
//           aria-label="Toggle navigation"
//         >
//           Home
//         </button>

//         <button
//           onClick={() => navigate(`/profile/${user.id}`)}
//           className="navbar-toggler"
//           type="button"
//           data-bs-toggle="collapse"
//           data-bs-target="#navbarNavAltMarkup"
//           aria-controls="navbarNavAltMarkup"
//           aria-expanded="false"
//           aria-label="Toggle navigation"
//         >
//           Profile
//         </button>

//         <button
//           onClick={() => navigate("/messages")}
//           className="navbar-toggler"
//           type="button"
//           data-bs-toggle="collapse"
//           data-bs-target="#navbarNavAltMarkup"
//           aria-controls="navbarNavAltMarkup"
//           aria-expanded="false"
//           aria-label="Toggle navigation"
//         >
//           Messages
//         </button>

//         {isAdminResult ? (
//           <button
//             onClick={() => navigate("/admin")}
//             className="navbar-toggler"
//             type="button"
//             data-bs-toggle="collapse"
//             data-bs-target="#navbarNavAltMarkup"
//             aria-controls="navbarNavAltMarkup"
//             aria-expanded="false"
//             aria-label="Toggle navigation"
//           >
//             Admin
//           </button>
//         ) : null}

//         {user ? (
//           <button
//             onClick={logoutUser}
//             className="navbar-toggler"
//             type="button"
//             data-bs-toggle="collapse"
//             data-bs-target="#navbarNavAltMarkup"
//             aria-controls="navbarNavAltMarkup"
//             aria-expanded="false"
//             aria-label="Toggle navigation"
//           >
//             Logout
//           </button>
//         ) : (
//           <button
//             onClick={() => navigate("/login")}
//             className="navbar-toggler"
//             type="button"
//             data-bs-toggle="collapse"
//             data-bs-target="#navbarNavAltMarkup"
//             aria-controls="navbarNavAltMarkup"
//             aria-expanded="false"
//             aria-label="Toggle navigation"
//           >
//             Login
//           </button>
//         )}
//       </div>
//     </div>
