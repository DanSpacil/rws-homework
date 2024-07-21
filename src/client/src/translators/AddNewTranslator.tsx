import { useState } from "react";
import useAddTranslator from "./useAddTranslator";
import { TranslatorPostModel, TranslatorStatus } from "./types";

function AddNewTranslator() {
  const addTranslator = useAddTranslator();
  const [name, setName] = useState<string>("");
  const [hourlyRate, setHourlyRate] = useState<string>("");
  const [status, setTranslatorStatus] = useState<TranslatorStatus>(TranslatorStatus.Applicant);
  const [creditCardNumber, setCreditCardNumber] = useState<string>("");

  const createNewTranslator = () => {
    const translator: TranslatorPostModel = {
      name: name,
      hourlyRate: hourlyRate,
      status: status,
      creditCardNumber: creditCardNumber,
    };
    addTranslator(translator);
  }

  return (
    <div className="new-translator">
      <label>Name<input type="text" value={name} onChange={e => setName(e.target.value)}/></label>
      <label>Rate (hourly)<input type="number" value={hourlyRate} onChange={e => setHourlyRate(e.target.value)}/></label>
      <label>
        Status
        <select name="translator-status" id="translator-status" onChange={e => setTranslatorStatus(Number(e.target.value))}>
          <option value={TranslatorStatus.Applicant}>Applicant</option>
          <option value={TranslatorStatus.Certified}>Certified</option>
          <option value={TranslatorStatus.Deleted}>Deleted</option>
        </select>
      </label>
      <label>Credit card number<input type="text" value={creditCardNumber} onChange={e => setCreditCardNumber(e.target.value)}/></label>
      <button onClick={createNewTranslator}>Add new translator</button>
  </div>)
}

export default AddNewTranslator;
