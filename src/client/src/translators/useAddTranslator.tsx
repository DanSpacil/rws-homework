import { TranslatorPostModel } from "./types";

const useAddTranslator = () => {
  const addTranslator = (translator: TranslatorPostModel) => {
    fetch("/api/translator/AddTranslator", {
      method: "POST",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json"
      },
      body: JSON.stringify(translator)
    });
  }

  return addTranslator;
}

export default useAddTranslator;
