import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from './components/Header'; // Assurez-vous que le chemin est correct
import Home from './components/Home'; // Assurez-vous que ce composant existe également
import Footer from './components/Footer'; // Assurez-vous que ce composant existe également

const App = () => {
  return (
    <div>
      <Router>
        <Header /> {/* Votre composant Header */}
        <Home />   {/* Votre composant Home */}
        <Footer /> {/* Votre composant Footer */}
        <Routes>
          {/* Définir les routes ici */}
          <Route path="/" element={<Home />} /> 
        </Routes>
      </Router>
    </div>
  );
};

export default App;
