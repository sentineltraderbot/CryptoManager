export class UserToken {
  constructor(tokenDecoded: any, token: string) {
    this.id = tokenDecoded.Id;
    this.email = tokenDecoded.Email;
    this.imageURL = tokenDecoded.PictureURL;
    this.username = tokenDecoded.Name;
    this.role =
      tokenDecoded[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];
    this.token = token;
    this.isAdmin =
      this.role != undefined ? this.role.indexOf("Administrator") > -1 : false;
  }

  id: string;
  email: string;
  token: string;
  username: string;
  imageURL: string;
  role: string[];
  isAdmin: boolean;
}
