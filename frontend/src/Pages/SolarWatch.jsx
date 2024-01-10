import { useState } from "react";
import axios from "axios";
const SolarWatch = () => {
  const [cityToCheck, setCity] = useState("");
  const [solarData, setSolarData] = useState(null);

  const handleSolarWatch = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.get(`http://localhost:5279/GetSolarData?cityName=${cityToCheck}`);
      setSolarData(response.data);
    } catch (error) {
      alert("Something went wrong!");
    }
  };

  return (
    <>
    <div className="form">
      <input
        type="text"
        placeholder="Please input where you want to check SolarWatch!"
        onChange={(e) => {
          setCity(e.target.value);
        }}
      />
      <button onClick={handleSolarWatch}>Start SolarWatch</button>
    </div>
    {solarData ? 
      <div className="SolarData">
        <p>City: {solarData.name}</p>
        <p>Sunrise at: {solarData.sunrise}</p>
        <p>Sunset at: {solarData.sunset}</p>
      </div> : <></>}
    </>
  );
};
export default SolarWatch;
