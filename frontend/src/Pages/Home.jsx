const Home = ({isLoggedIn}) => {
    return(
    <div className="homeDiv">
        {isLoggedIn ? <p>Welcome click on SolarWatch to use the app!</p> : <p>You are currently not logged in! Please login to use SolarWatch!</p>}
    </div>)
}
export default Home;