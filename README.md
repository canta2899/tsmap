# Tsmap

The project implements a tool that converts **C# (POCO) classes** to **typescript interfaces**. It is aimed at backend projects developed in .NET that require to share a type system with a frontend developed in typescript.

All the main functionalities have been implemented so the project can be used in order to test its functionalities in various scenarios.

## Usage

In order to map C# classes you need to install `TypescriptMapper.Annotations` in your project and apply the `[TsMap]` attributes to all the classes that you want to map to typescript. If you want to exclude a specific property of a class you can add the `[TsExclude]` attribute to the property.

Once you did that you have to **build you project**. Then, you can run `TypescriptMapper.Cli` passing a path to the DLL which contains the models you want to map. If you want the app not to apply camelCasing to your properties you can add the `ignore-case` flag to the command.

The command will print the whole `types.ts` file to the standard output, so you can use *shell output redirection* in order to redirect it to your desired file:

```
cd TypescriptMapper.Cli
dotnet run ../../MyProjectSolution/MyProject/bin/Release/MyProject.dll ignore-case > ./types.ts
```

## Example

Here's an example of some C# classes with their typescript equivalent converted using TypescriptMapper.

```cs
[TypescriptMapper.Annotations.TsMap]
public class AuthenticationRequest
{
    public User User { get; set; }

    public string Password { get; set; }
}

// this won't be mapped
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
} 

[TypescriptMapper.Annotations.TsMap]
public class AuthenticationResponse : MessageResponse
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}

[TypescriptMapper.Annotations.TsMap]
public class PasswordChangeRequest
{
    public string OldPassword { get; set; }

    public string NewPassword { get; set; }

    [TsExclude]
    public string Id {get; set;}
}

[TypescriptMapper.Annotations.TsMap]
public class PasswordResetRequest
{
    public string UserName { get; set; }

    public string NewPassword { get; set; }
}

[TypescriptMapper.Annotations.TsMap]
public class RefreshRequest
{
    public string RefreshToken { get; set; }
}

```

```typescript
export interface AuthenticationRequest {
  User?: any;
  Password?: string;
}

export interface AuthenticationResponse extends MessageResponse {
  AccessToken?: string;
  RefreshToken?: string;
}

export interface PasswordChangeRequest {
  OldPassword?: string;
  NewPassword?: string;
}

export interface PasswordResetRequest {
  UserName?: string;
  NewPassword?: string;
}

export interface RefreshRequest {
  RefreshToken?: string;
}
```

