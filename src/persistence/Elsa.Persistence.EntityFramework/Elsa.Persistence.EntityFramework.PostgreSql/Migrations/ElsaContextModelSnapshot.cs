﻿// <auto-generated />
using System;
using Elsa.Persistence.EntityFramework.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Elsa.Persistence.EntityFramework.PostgreSql.Migrations
{
    [DbContext(typeof(ElsaContext))]
    partial class ElsaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Elsa")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Elsa.Models.Bookmark", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ActivityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ActivityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CorrelationId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ModelType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.Property<string>("WorkflowInstanceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId")
                        .HasDatabaseName("IX_Bookmark_ActivityId");

                    b.HasIndex("ActivityType")
                        .HasDatabaseName("IX_Bookmark_ActivityType");

                    b.HasIndex("CorrelationId")
                        .HasDatabaseName("IX_Bookmark_CorrelationId");

                    b.HasIndex("Hash")
                        .HasDatabaseName("IX_Bookmark_Hash");

                    b.HasIndex("TenantId")
                        .HasDatabaseName("IX_Bookmark_TenantId");

                    b.HasIndex("WorkflowInstanceId")
                        .HasDatabaseName("IX_Bookmark_WorkflowInstanceId");

                    b.HasIndex("ActivityType", "TenantId", "Hash")
                        .HasDatabaseName("IX_Bookmark_ActivityType_TenantId_Hash");

                    b.HasIndex("Hash", "CorrelationId", "TenantId")
                        .HasDatabaseName("IX_Bookmark_Hash_CorrelationId_TenantId");

                    b.ToTable("Bookmarks");
                });

            modelBuilder.Entity("Elsa.Models.Trigger", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ActivityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ActivityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ModelType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.Property<string>("WorkflowDefinitionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId")
                        .HasDatabaseName("IX_Trigger_ActivityId");

                    b.HasIndex("ActivityType")
                        .HasDatabaseName("IX_Trigger_ActivityType");

                    b.HasIndex("Hash")
                        .HasDatabaseName("IX_Trigger_Hash");

                    b.HasIndex("TenantId")
                        .HasDatabaseName("IX_Trigger_TenantId");

                    b.HasIndex("WorkflowDefinitionId")
                        .HasDatabaseName("IX_Trigger_WorkflowDefinitionId");

                    b.HasIndex("Hash", "TenantId")
                        .HasDatabaseName("IX_Trigger_Hash_TenantId");

                    b.HasIndex("ActivityType", "TenantId", "Hash")
                        .HasDatabaseName("IX_Trigger_ActivityType_TenantId_Hash");

                    b.ToTable("Triggers");
                });

            modelBuilder.Entity("Elsa.Models.WorkflowDefinition", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<string>("DefinitionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("DeleteCompletedInstances")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<bool>("IsLatest")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSingleton")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PersistenceBehavior")
                        .HasColumnType("integer");

                    b.Property<string>("Tag")
                        .HasColumnType("text");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IsLatest")
                        .HasDatabaseName("IX_WorkflowDefinition_IsLatest");

                    b.HasIndex("IsPublished")
                        .HasDatabaseName("IX_WorkflowDefinition_IsPublished");

                    b.HasIndex("Name")
                        .HasDatabaseName("IX_WorkflowDefinition_Name");

                    b.HasIndex("Tag")
                        .HasDatabaseName("IX_WorkflowDefinition_Tag");

                    b.HasIndex("TenantId")
                        .HasDatabaseName("IX_WorkflowDefinition_TenantId");

                    b.HasIndex("Version")
                        .HasDatabaseName("IX_WorkflowDefinition_Version");

                    b.HasIndex("DefinitionId", "Version")
                        .IsUnique()
                        .HasDatabaseName("IX_WorkflowDefinition_DefinitionId_VersionId");

                    b.ToTable("WorkflowDefinitions");
                });

            modelBuilder.Entity("Elsa.Models.WorkflowExecutionLogRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ActivityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ActivityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<string>("EventName")
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("WorkflowInstanceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId")
                        .HasDatabaseName("IX_WorkflowExecutionLogRecord_ActivityId");

                    b.HasIndex("ActivityType")
                        .HasDatabaseName("IX_WorkflowExecutionLogRecord_ActivityType");

                    b.HasIndex("TenantId")
                        .HasDatabaseName("IX_WorkflowExecutionLogRecord_TenantId");

                    b.HasIndex("Timestamp")
                        .HasDatabaseName("IX_WorkflowExecutionLogRecord_Timestamp");

                    b.HasIndex("WorkflowInstanceId")
                        .HasDatabaseName("IX_WorkflowExecutionLogRecord_WorkflowInstanceId");

                    b.ToTable("WorkflowExecutionLogRecords");
                });

            modelBuilder.Entity("Elsa.Models.WorkflowInstance", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("CancelledAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ContextId")
                        .HasColumnType("text");

                    b.Property<string>("ContextType")
                        .HasColumnType("text");

                    b.Property<string>("CorrelationId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<string>("DefinitionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DefinitionVersionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("FaultedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("FinishedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastExecutedActivityId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("LastExecutedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.Property<int>("WorkflowStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ContextId")
                        .HasDatabaseName("IX_WorkflowInstance_ContextId");

                    b.HasIndex("ContextType")
                        .HasDatabaseName("IX_WorkflowInstance_ContextType");

                    b.HasIndex("CorrelationId")
                        .HasDatabaseName("IX_WorkflowInstance_CorrelationId");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_WorkflowInstance_CreatedAt");

                    b.HasIndex("DefinitionId")
                        .HasDatabaseName("IX_WorkflowInstance_DefinitionId");

                    b.HasIndex("DefinitionVersionId")
                        .HasDatabaseName("IX_WorkflowInstance_DefinitionVersionId");

                    b.HasIndex("FaultedAt")
                        .HasDatabaseName("IX_WorkflowInstance_FaultedAt");

                    b.HasIndex("FinishedAt")
                        .HasDatabaseName("IX_WorkflowInstance_FinishedAt");

                    b.HasIndex("LastExecutedAt")
                        .HasDatabaseName("IX_WorkflowInstance_LastExecutedAt");

                    b.HasIndex("Name")
                        .HasDatabaseName("IX_WorkflowInstance_Name");

                    b.HasIndex("TenantId")
                        .HasDatabaseName("IX_WorkflowInstance_TenantId");

                    b.HasIndex("WorkflowStatus")
                        .HasDatabaseName("IX_WorkflowInstance_WorkflowStatus");

                    b.HasIndex("WorkflowStatus", "DefinitionId")
                        .HasDatabaseName("IX_WorkflowInstance_WorkflowStatus_DefinitionId");

                    b.HasIndex("WorkflowStatus", "DefinitionId", "Version")
                        .HasDatabaseName("IX_WorkflowInstance_WorkflowStatus_DefinitionId_Version");

                    b.ToTable("WorkflowInstances");
                });
#pragma warning restore 612, 618
        }
    }
}
