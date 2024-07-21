import { TranslatorStatus } from "./types";
import useTranslatorList from "./useTranslatorList";

const TranslatorList = () => {
  const translators = useTranslatorList();
  const getStatus = (status: TranslatorStatus): string => {
    const statusName = TranslatorStatus[status]
    return statusName;

  }
  return (
    <div className="translator-list">
      <h2>Current list of translators available</h2>
        <table>
          <thead>
         <tr>
          <th>Name</th>
          <th>Status</th>
          <th>Hourly rate</th>
         </tr>
         </thead>
          <tbody>
          {translators.map(t => {
            return (
            <tr key={t.id}>
                <td>{t.name}</td>
                <td>{getStatus(t.status)}</td>
                <td>{t.hourlyRate}</td>
            </tr>)
          })}
          </tbody>
        </table>
      </div>
  );
}

export default TranslatorList;
