import './App.css';
import AddNewTranslator from './translators/AddNewTranslator';
import TranslatorList from './translators/TranslatorList';

function App() {

  return (
    <div className="App">
      <h1>Translator management</h1>
      <article>
      <TranslatorList />
      <AddNewTranslator />
      </article>
    </div>
  );
}

export default App;
