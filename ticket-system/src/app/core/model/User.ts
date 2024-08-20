import { Roles } from './enums/Roles';

export class User {
  userId: number;
  fullName: string;
  mobileNumber: string;
  email: string;
  dob: string;
  address: string;
  userImage: string;
  fileName: string;
  isActive: boolean;
  token: string;
  createdAt: string;
  upstringdAt: string;
  role: Roles;
  userName: string;
  password: string;

  constructor() {
    this.userId = -1;
    this.fullName = '';
    this.mobileNumber = '';
    this.email = '';
    this.userName = '';
    this.dob = '';
    this.address = '';
    this.userImage = '';
    this.isActive = false;
    this.token = '';
    this.createdAt = '';
    this.upstringdAt = '';
    this.role = 1;
    this.fileName = '';
    this.password = '';
  }
}
