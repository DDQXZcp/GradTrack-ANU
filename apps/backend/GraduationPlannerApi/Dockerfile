# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /out --no-self-contained -r linux-x64

# Lambda runtime
FROM public.ecr.aws/lambda/dotnet:8
# Copy published app
COPY --from=build /out ${LAMBDA_TASK_ROOT}/
# The Lambda .NET base image runs the assembly named below
CMD ["GraduationPlannerApi"]