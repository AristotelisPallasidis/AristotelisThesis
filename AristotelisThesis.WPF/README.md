# AristotelisThesis.WPF

## Overview
This is the **Presentation Layer** built with Windows Presentation Foundation (WPF). It serves as the front-end for the biometric identity recognition system.

## Responsibilities
- **MVVM Architecture**: Heavily uses the Model-View-ViewModel pattern to separate UI design (`Views`, `Controls`) from application logic (`ViewModels`).
- **Dependency Injection**: Utilizes `Microsoft.Extensions.DependencyInjection` configured in `App.xaml.cs` to resolve services, VMs, and Navigation mechanisms.
- **Biometric Processing**: Implements OpenCvSharp processing (via components like `LoginWithFaceViewModel`) to handle camera feeds, perform facial/palmprint detection, and recognize identities against trained models.
- **State & Navigation**: Handles user state tracking (Login/Logout) and dynamic view switching through Renavigator commands and View factories.

## Multi-Stage Registration and Recognition Flows
The project contains complex specific flows for mapping users to biometric elements:
- A multi-step registration flow: Information -> Palmprint Instructions -> Palmprint Capture -> Face Instructions -> Face Capture.
- Login variations: Login with credentials, login with Face, and login with Palmprint.

## Key Relationships
- **Depends on**: `AristotelisThesis.Domain` (for Models) and `AristotelisThesis.EntityFramework` (for the actual DB contexts).
- Integrates `OpenCvSharp` and `OpenCvSharp.WpfExtensions` libraries to map internal computer vision frames directly to WPF Image resources.
