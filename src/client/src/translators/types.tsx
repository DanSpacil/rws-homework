export enum TranslatorStatus {
  Applicant = 0,
  Certified = 1,
  Deleted = 2
}

export type Translator = {
  id: number;
  name: string;
  hourlyRate: string;
  status: number;
  creditCardNumber: string;
}

export type TranslatorPostModel = {
  name: string;
  hourlyRate: string;
  status: number;
  creditCardNumber: string;
}
