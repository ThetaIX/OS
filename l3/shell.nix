{ pkgs ? import <nixpkgs> {} }:

pkgs.mkShell {
  buildInputs = [
    pkgs.linuxHeaders          
    pkgs.linuxPackages.kernel  
    pkgs.gcc                   
    pkgs.gnumake               
  ];
}
