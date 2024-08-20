import { Roles } from './enums/Roles';

export class RegisterModal {
  userId?: number;
  fullName: string;
  userName: string;
  mobileNumber: string;
  dob: string;
  email: string;
  password?: string;
  address: string;
  fileName?: string;
  role: Roles;
  userImage: string;
  isActive: boolean;

  constructor() {
    this.userId = -1;
    this.address = '';
    this.userName = '';
    this.mobileNumber = '';
    this.dob = '';
    this.email = '';
    this.password = '';
    this.fileName = '';
    this.role = 1;
    this.fullName = '';
    this.userImage = '';
    this.isActive = true;
  }
}
