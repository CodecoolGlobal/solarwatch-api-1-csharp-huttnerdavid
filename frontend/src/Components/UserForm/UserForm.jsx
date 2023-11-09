import { useState, useEffect } from "react";
import Loading from "../Loading";

const EmployeeForm = ({ onSave, user, onCancel }) => {

  const [loading, setLoading] = useState(false);
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");

  const onSubmit = (e) => {
    e.preventDefault();

    if (user) {
      return onSave({
        ...user,
        userName,
        password,
        email
      });
    }

    return onSave({
      userName,
      password,
      email
    });
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <form className="EmployeeForm" onSubmit={onSubmit}>
      
      <div className="control">
        <label htmlFor="userName">Username:</label>
        <input
          value={userName}
          onChange={(e) => setUserName(e.target.value)}
          name="userName"
          id="userName"
        />
      </div>

      <div className="control">
        <label htmlFor="password">Password:</label>
        <input
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          name="password"
          id="password"
        />
      </div>

      <div className="control">
        <label htmlFor="email">Email:</label>
        <input
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          name="email"
          id="email"
        />
      </div>
      <div className="buttons">
        <button type="submit">
          {user ? "Update User" : "Create User"}
        </button>
        <button type="button" onClick = { onCancel }>
          Cancel
        </button>
      </div>
    </form>
  );
};

export default EmployeeForm;
