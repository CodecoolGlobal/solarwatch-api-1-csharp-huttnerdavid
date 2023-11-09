import { useEffect, useState } from "react";
import Loading from "../Components/Loading";
import UserForm from "../Components/UserForm";
import { useNavigate } from "react-router-dom";

const createEmployee = (user) => {
  const jsonPayload = JSON.stringify(user);
  return fetch("http://localhost:5279/Register", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: jsonPayload,
  }).then((res) => res.json());
};

const UserRegistration = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleCancel = () => {
    navigate("/");
  }

  const handleSubmit = (user) => {
    console.log(user);
    setLoading(true);
    createEmployee(user, setData);
    setLoading(false);
    navigate("/");
    alert("Successfully registered!");
  }

  return (
    <div>
      {loading ? (
        <Loading />
      ) : (
        <UserForm
          onSave = { handleSubmit }
          onCancel = { handleCancel }
        />
      )}
    </div>
  );
};

export default UserRegistration;