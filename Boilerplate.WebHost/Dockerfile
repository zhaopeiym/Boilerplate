#基础镜像（用来构建镜像） 用sdk 是因为需要用到dotnet new --install命令
FROM microsoft/dotnet:2.1-sdk AS base
WORKDIR /app
EXPOSE 80
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

#编译（临时镜像，主要用来编译发布项目）
FROM microsoft/dotnet:2.1-sdk AS publish
WORKDIR /src
COPY . .
WORKDIR /src
RUN dotnet publish -c Release -o /app

#构建镜像
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Boilerplate.WebHost.dll"]