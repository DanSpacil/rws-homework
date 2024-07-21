import { useEffect, useState } from "react"
import { Translator } from "./types";

const useTranslatorList = (): Translator[] => {
  const [translators, setTranslators] = useState<Translator[]>([]);
  const fetchTranslators = () => {
    fetch("/api/translator/GetTranslators")
      .then(response => response.json())
      .then(data => setTranslators(data))
  }

  useEffect(() => {
    fetchTranslators();
  },[])

  return translators
  
}

export default useTranslatorList;
