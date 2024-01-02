const Home = ({isLoggedIn}) => {
    return(
    <div>
        {isLoggedIn ? <p>Welcome click on SolarWatch to use the app!</p> : <p>Please login!</p>}
    </div>)
}
export default Home;